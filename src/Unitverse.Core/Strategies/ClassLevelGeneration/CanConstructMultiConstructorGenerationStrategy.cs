﻿namespace Unitverse.Core.Strategies.ClassLevelGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Unitverse.Core.Frameworks;
    using Unitverse.Core.Helpers;
    using Unitverse.Core.Models;
    using Unitverse.Core.Options;

    public class CanConstructMultiConstructorGenerationStrategy : IGenerationStrategy<ClassModel>
    {
        private readonly IFrameworkSet _frameworkSet;

        public CanConstructMultiConstructorGenerationStrategy(IFrameworkSet frameworkSet)
        {
            _frameworkSet = frameworkSet ?? throw new ArgumentNullException(nameof(frameworkSet));
        }

        public bool IsExclusive => false;

        public int Priority => 1;

        public Func<IStrategyOptions, bool> IsEnabled => x => x.ConstructorChecksAreEnabled;

        public bool CanHandle(ClassModel method, ClassModel model)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return model.Declaration.ChildNodes().OfType<ConstructorDeclarationSyntax>().Count(x => x.Modifiers.All(m => !m.IsKind(SyntaxKind.StaticKeyword))) > 1 && !model.IsStatic;
        }

        public IEnumerable<SectionedMethodHandler> Create(ClassModel method, ClassModel model, NamingContext namingContext)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var generatedMethod = _frameworkSet.CreateTestMethod(_frameworkSet.NamingProvider.CanConstruct, namingContext, false, false, "Checks that instance construction works.");

            bool isFirst = true;
            foreach (var constructor in model.Constructors)
            {
                var tokenList = constructor.Parameters.Select(parameter => model.GetConstructorFieldReference(parameter, _frameworkSet)).Cast<ExpressionSyntax>().ToList();

                var creationExpression = Generate.ObjectCreation(model.TypeSyntax, tokenList.ToArray());

                if (isFirst)
                {
                    generatedMethod.Act(Generate.ImplicitlyTypedVariableDeclaration("instance", creationExpression));
                    isFirst = false;
                }
                else
                {
                    generatedMethod.Act(Generate.Statement(SyntaxFactory.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, SyntaxFactory.IdentifierName("instance"), creationExpression)));
                }

                generatedMethod.Assert(_frameworkSet.AssertionFramework.AssertNotNull(SyntaxFactory.IdentifierName("instance")));
                generatedMethod.BlankLine();
            }

            yield return generatedMethod;
        }
    }
}