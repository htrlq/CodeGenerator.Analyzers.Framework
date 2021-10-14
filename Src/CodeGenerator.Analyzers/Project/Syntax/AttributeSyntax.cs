using Microsoft.CodeAnalysis;
using XAttributeSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.AttributeSyntax;

namespace CodeGenerator.Analyzers.Framework.Project.Syntax
{
    public class AttributeSyntax
    {
        public string Name { get; }
        public string FullName { get; }
        public object[] ConstantValues { get; }

        public AttributeSyntax(XAttributeSyntax attributeSyntax, Compilation compilation)
        {
            var semantic = compilation.GetSemanticModel(attributeSyntax.SyntaxTree);
            var _attributeSymbol = semantic.GetSymbolInfo(attributeSyntax);

            Name = _attributeSymbol.Symbol.Name;
            FullName = _attributeSymbol.Symbol.ContainingType.ToDisplayString();
            ConstantValues = attributeSyntax.GetCustomAttributeConstantValue(semantic);
        }
    }
}
