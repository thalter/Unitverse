﻿namespace Unitverse.Commands
{
    using System;
    using System.ComponentModel.Design;
    using System.Globalization;
    using System.Windows.Input;
    using EnvDTE;
    using EnvDTE80;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Text;
    using Microsoft.VisualStudio.Shell;
    using Unitverse.Core.Strategies.InterfaceGeneration;
    using Unitverse.Helper;
    using Task = System.Threading.Tasks.Task;

    /// <summary>
    /// Command handler.
    /// </summary>
    internal sealed class GenerateTestForSymbolCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        private const int CommandId = 258;

        private const int RegenerateCommandId = 259;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        private static readonly Guid CommandSet = new Guid("63d6b7b1-4f20-4519-9f56-09f9e220fd1b");

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        private static GenerateTestForSymbolCommand _instance;

        private static DTE2 _dte;

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly IUnitTestGeneratorPackage _package;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateTestForSymbolCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file).
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private GenerateTestForSymbolCommand(IUnitTestGeneratorPackage package, OleMenuCommandService commandService)
        {
            _package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandId = new CommandID(CommandSet, CommandId);
            var regenerationMenuCommandId = new CommandID(CommandSet, RegenerateCommandId);

            var menuItem = new OleMenuCommand(Execute, menuCommandId);
            var regenerationMenuItem = new OleMenuCommand(ExecuteRegeneration, regenerationMenuCommandId);

            menuItem.BeforeQueryStatus += (s, e) =>
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                var textView = TextViewHelper.GetTextView(ServiceProvider);

                var methodTask = package.JoinableTaskFactory.RunAsync(async () => await TextViewHelper.GetTargetSymbolAsync(textView).ConfigureAwait(true));
                var tuple = methodTask.Join();
                var symbol = tuple?.Item2;
                var baseType = tuple?.Item3;
                menuItem.Visible = false;
                regenerationMenuItem.Visible = false;
                if (symbol != null)
                {
                    menuItem.Visible = true;
                    regenerationMenuItem.Visible = Keyboard.IsKeyDown(Key.LeftShift);
                    if (symbol.Kind == SymbolKind.NamedType)
                    {
                        menuItem.Text = "Generate tests for type '" + symbol.Name + "'...";
                        regenerationMenuItem.Text = "Regenerate tests for type '" + symbol.Name + "'...";
                    }
                    else if (string.Equals(symbol.Name, ".ctor", StringComparison.OrdinalIgnoreCase))
                    {
                        menuItem.Text = "Generate test for all constructors...";
                        regenerationMenuItem.Text = "Regenerate test for all constructors...";
                    }
                    else
                    {
                        menuItem.Text = "Generate test for " + symbol.Kind.ToString().ToLower(CultureInfo.CurrentCulture) + " '" + symbol.Name + "'...";
                        regenerationMenuItem.Text = "Regenerate test for " + symbol.Kind.ToString().ToLower(CultureInfo.CurrentCulture) + " '" + symbol.Name + "'...";
                    }
                }
                else if (baseType.HasValue)
                {
                    if (InterfaceGenerationStrategyFactory.Supports(baseType.Value))
                    {
                        menuItem.Visible = true;
                        regenerationMenuItem.Visible = Keyboard.IsKeyDown(Key.LeftShift);
                        menuItem.Text = "Generate tests for base type '" + baseType.Value.Type.Name + "'...";
                        regenerationMenuItem.Text = "Regenerate tests for base type '" + baseType.Value.Type.Name + "'...";
                    }
                }
            };

            commandService.AddCommand(menuItem);
            commandService.AddCommand(regenerationMenuItem);
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider ServiceProvider => _package;

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task InitializeAsync(UnitTestGeneratorPackage package)
        {
            await package.JoinableTaskFactory.SwitchToMainThreadAsync();
#pragma warning disable VSSDK006 // these services are always available
            _dte = (DTE2)await package.GetServiceAsync(typeof(DTE)).ConfigureAwait(true);

            var commandService = await package.GetServiceAsync(typeof(IMenuCommandService)).ConfigureAwait(true) as OleMenuCommandService;
            _instance = new GenerateTestForSymbolCommand(package, commandService);
        }

        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            Execute(false);
        }

        private void ExecuteRegeneration(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            Execute(true);
        }

        private void Execute(bool withRegeneration)
        {
            Attempt.Action(
                () =>
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                var textView = TextViewHelper.GetTextView(ServiceProvider);
                if (textView == null)
                {
                    throw new InvalidOperationException("Could not find the text view");
                }

                var caretPosition = textView.Caret.Position.BufferPosition;
                var document = caretPosition.Snapshot.GetOpenDocumentInCurrentContextWithChanges();

                var messageLogger = new AggregateLogger();
                messageLogger.Initialize();

                var source = new ProjectItemModel(VsProjectHelper.GetProjectItem(document.FilePath));
                var mapping = ProjectMappingFactory.CreateMappingFor(source.Project, _package.Options, true, false, messageLogger);
                if (mapping == null)
                {
                    return;
                }

                var generationItem = new GenerationItem(source, mapping);

                _ = _package.JoinableTaskFactory.RunAsync(
                    () => Attempt.ActionAsync(
                        async () =>
                    {
                        var methodSymbol = await TextViewHelper.GetTargetSymbolAsync(textView).ConfigureAwait(true);

                        if (methodSymbol != null)
                        {
                            generationItem.SourceNode = methodSymbol.Item1;
                            generationItem.SourceSymbol = methodSymbol.Item2;

                            await CodeGenerator.GenerateCodeAsync(new[] { generationItem }, withRegeneration, _package, messageLogger).ConfigureAwait(true);
                        }
                    }, _package));
            }, _package);
        }
    }
}
