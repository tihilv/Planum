using System;
using System.Collections.Generic;
using Bundle.Uml;
using Language.Common;
using Language.Processing;

namespace Planum.Ui.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";

        public DocumentViewModel Document { get; set; }
        
        public MainWindowViewModel()
        {
            var builder = new DefaultBundleBuilder();
            builder.RegisterBundle(UmlBundle.Instance);

            Document = new DocumentViewModel(new TextScript(GetSimpleScript()), builder);
        }
        
        private static List<String> GetSimpleScript()
        {
            var sb = new List<String>();
            sb.Add("@startuml");
            sb.Add("left to right direction");
            sb.Add("actor \"Food Critic\" as fc");
            sb.Add("rectangle Restaurant {");
            sb.Add("    usecase \"Eat Food\" as UC1");
            sb.Add("    usecase \"Pay for Food\" as UC2");
            sb.Add("    usecase \"Drink\" as UC3");
            sb.Add("}");
            sb.Add("fc --> UC1");
            sb.Add("fc --> UC2");
            sb.Add("fc --> UC3");
            sb.Add("@enduml");
            return sb;
        }

    }
}