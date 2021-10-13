using XParameterSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax;

namespace CodeGenerator.Analyzers.Framework.Project.Syntax
{
    public class ParameterSyntax
    {
        private XParameterSyntax _parameterSyntax;

        public ParameterSyntax(XParameterSyntax parameterSyntax)
        {
            _parameterSyntax = parameterSyntax;
        }

        public string Type { get => _parameterSyntax.Type.ToFullString(); }
        public string Name { get => _parameterSyntax.Identifier.ValueText; }
    }
}
