using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Microsoft.CodeAnalysis
{
    public static class SyntaxtStaticExtension
    {
        private static bool CheckAttribute(AttributeSyntax _attribute, Compilation compilation, string _attributeName)
        {
            var semantic = compilation.GetSemanticModel(_attribute.SyntaxTree);
            var _attributeSymbol = semantic.GetSymbolInfo(_attribute);
            var att = _attributeSymbol.Symbol.ContainingType.ToDisplayString();

            return _attributeSymbol.Symbol.ContainingType.ToDisplayString() == _attributeName;
        }

        public static AttributeSyntax GetCustomAttribute(this AttributeListSyntax node, Compilation compilation, string attributeFullName)
        {
            var attributeSymbol = compilation.GetTypeByMetadataName(attributeFullName);

            if (attributeSymbol != null)
            {
                var currentAttribute = node.Attributes.FirstOrDefault(_attribute => CheckAttribute(_attribute, compilation, attributeFullName));

                return currentAttribute;
            }

            return null;
        }

        public static object[] GetCustomAttributeConstantValue(this AttributeSyntax node, SemanticModel semanticModel)
        {
            var args = node.ArgumentList;
            var result = new object[args.Arguments.Count];

            for(var i = 0; i < args.Arguments.Count; i++)
            {
                var arg = args.Arguments[i];

                if (arg.Expression is LiteralExpressionSyntax syntax)
                {
                    var value = semanticModel.GetConstantValue(syntax);
                    result[i] = value.Value;
                }
            }

            return result;
        }

        public static string Comment(this SyntaxNode node)
        {
            var trivia = node.GetLeadingTrivia();
            var comment = trivia.ToFullString();

            return comment;
        }

        public static bool IsPublic(this ClassDeclarationSyntax node)
        {
            var token = SyntaxFactory.Token(SyntaxKind.PublicKeyword);
            return node.Modifiers.Any(_modifier => _modifier.ValueText == token.ValueText);
        }

        public static bool IsStatic(this ClassDeclarationSyntax node)
        {
            var token = SyntaxFactory.Token(SyntaxKind.StaticKeyword);
            return node.Modifiers.Any(_modifier => _modifier.ValueText == token.ValueText);
        }

        public static bool IsInternal(this ClassDeclarationSyntax node)
        {
            var token = SyntaxFactory.Token(SyntaxKind.InternalKeyword);
            return node.Modifiers.Any(_modifier => _modifier.ValueText == token.ValueText);
        }

        public static bool IsSealed(this ClassDeclarationSyntax node)
        {
            var token = SyntaxFactory.Token(SyntaxKind.SealedKeyword);
            return node.Modifiers.Any(_modifier => _modifier.ValueText == token.ValueText);
        }

        public static bool IsPartial(this ClassDeclarationSyntax node)
        {
            var token = SyntaxFactory.Token(SyntaxKind.PartialKeyword);
            return node.Modifiers.Any(_modifier => _modifier.ValueText == token.ValueText);
        }

        public static bool IsAbstract(this ClassDeclarationSyntax node)
        {
            var token = SyntaxFactory.Token(SyntaxKind.AbstractKeyword);
            return node.Modifiers.Any(_modifier => _modifier.ValueText == token.ValueText);
        }


        public static bool IsPublic(this MethodDeclarationSyntax node)
        {
            var token = SyntaxFactory.Token(SyntaxKind.PublicKeyword);
            return node.Modifiers.Any(_modifier => _modifier.ValueText == token.ValueText);
        }

        public static bool IsStatic(this MethodDeclarationSyntax node)
        {
            var token = SyntaxFactory.Token(SyntaxKind.StaticKeyword);
            return node.Modifiers.Any(_modifier => _modifier.ValueText == token.ValueText);
        }

        public static bool IsInternal(this MethodDeclarationSyntax node)
        {
            var token = SyntaxFactory.Token(SyntaxKind.InternalKeyword);
            return node.Modifiers.Any(_modifier => _modifier.ValueText == token.ValueText);
        }

        public static bool IsPrivate(this MethodDeclarationSyntax node)
        {
            var token = SyntaxFactory.Token(SyntaxKind.PrivateKeyword);
            return node.Modifiers.Any(_modifier => _modifier.ValueText == token.ValueText);
        }

        public static bool IsVirtual(this MethodDeclarationSyntax node)
        {
            var token = SyntaxFactory.Token(SyntaxKind.VirtualKeyword);
            return node.Modifiers.Any(_modifier => _modifier.ValueText == token.ValueText);
        }

        public static bool IsAbstract(this MethodDeclarationSyntax node)
        {
            var token = SyntaxFactory.Token(SyntaxKind.AbstractKeyword);
            return node.Modifiers.Any(_modifier => _modifier.ValueText == token.ValueText);
        }

        public static bool IsProtected(this MethodDeclarationSyntax node)
        {
            var token = SyntaxFactory.Token(SyntaxKind.ProtectedKeyword);
            return node.Modifiers.Any(_modifier => _modifier.ValueText == token.ValueText);
        }

        public static bool IsBaseTypes(this ClassDeclarationSyntax node)
        {
            return node.DescendantNodes().OfType<BaseListSyntax>().Any();
        }

#if DEBUG
        public static void SaveFile(this ClassDeclarationSyntax node)
        {
            var fullPath = Path.Combine(Environment.GetEnvironmentVariable("ASPNETCORE_TARGETDIR", EnvironmentVariableTarget.Machine), node.Identifier.ValueText + ".cs");

            File.WriteAllText(fullPath, node.ToFullString());
        }
#endif
    }
}
