﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (https://www.specflow.org/).
//      SpecFlow Version:3.9.0.0
//      SpecFlow Generator Version:3.9.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Unitverse.Specs.Strategies.MethodGeneration
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.9.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("NullParameterCheckMethodGeneration")]
    public partial class NullParameterCheckMethodGenerationFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = ((string[])(null));
        
#line 1 "NullParameterCheckMethodGeneration.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Strategies/MethodGeneration", "NullParameterCheckMethodGeneration", "\tI am checking the Null Parameter Check Method Generation strategy", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.OneTimeTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void TestTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<NUnit.Framework.TestContext>(NUnit.Framework.TestContext.CurrentContext);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Null Parameter Check Method Generation")]
        public virtual void NullParameterCheckMethodGeneration()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Null Parameter Check Method Generation", null, tagsOfScenario, argumentsOfScenario, this._featureTags);
#line 5
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 6
 testRunner.Given("I have a class defined as", @"using System;
using System.Collections.Generic;
using System.Linq;

public static class Generate
{
public static IList<string> Test1(out IEnumerable<string> testing)
{
	testing = 1
}

public static IList<string> Test2(ref IEnumerable<string> testing)
{
	testing = 1
}

   public static IList<string> Arguments(params string[] expressions)
   {
       return ArgumentList(expressions);
   }

   public static IList<string> Arguments(IEnumerable<string> expressions, IEnumerable<string> expressions2)
   {
       return ArgumentList(expressions);
   }

   private static IList<string> ArgumentList(params string[] expressions)
   {
       return ArgumentList(expressions.AsEnumerable());
   }

   private static IList<string> ArgumentList(IEnumerable<string> expressions)
   {
       var tokens = new List<string>();
       foreach (var expression in expressions)
       {
           if (tokens.Count > 0)
           {
               tokens.Add("","");
           }

           tokens.Add(expression);
       }

       return tokens;
   }
}", ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 56
 testRunner.And("I set my test framework to \'XUnit\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 57
 testRunner.And("I set my mock framework to \'Moq\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 58
 testRunner.When("I generate tests for the method using the strategy \'NullParameterCheckMethodGener" +
                        "ationStrategy\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
                TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                            "methodName",
                            "methodStatement"});
                table1.AddRow(new string[] {
                            "CannotCallArgumentsWithExpressionsWithNullExpressions",
                            "Assert.Throws<ArgumentNullException>(() => Generate.Arguments(default(string[])))" +
                                ";"});
                table1.AddRow(new string[] {
                            "CannotCallArgumentsWithExpressionsAndExpressions2WithNullExpressions",
                            "Assert.Throws<ArgumentNullException>(()=>Generate.Arguments(default(IEnumerable<s" +
                                "tring>),new[]{{{{AnyString}}}, {{{AnyString}}}, {{{AnyString}}}}));"});
                table1.AddRow(new string[] {
                            "CannotCallArgumentsWithExpressionsAndExpressions2WithNullExpressions2",
                            "Assert.Throws<ArgumentNullException>(()=>Generate.Arguments(new[]{{{{AnyString}}}" +
                                ", {{{AnyString}}}, {{{AnyString}}}}, default(IEnumerable<string>)));"});
                table1.AddRow(new string[] {
                            "CannotCallTest2WithNullTesting",
                            "Assert.Throws<ArgumentNullException>(()=>Generate.Test2(reftesting));"});
#line 59
 testRunner.Then("I expect methods with statements like:", ((string)(null)), table1, "Then ");
#line hidden
#line 65
 testRunner.And("I expect no method with a name like \'.*Test1.*\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
