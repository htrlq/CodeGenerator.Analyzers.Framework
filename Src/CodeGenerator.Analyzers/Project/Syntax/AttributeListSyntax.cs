using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using XAttributeListSyntax = Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax;

namespace CodeGenerator.Analyzers.Framework.Project.Syntax
{
    public class AttributeListSyntax
    {
        public ICollection<AttributeSyntax> Attributes { get; }

        public AttributeListSyntax(XAttributeListSyntax attributeListSyntax, Compilation compilation)
        {
            Attributes = attributeListSyntax.Attributes.Select(_syntaxt => new AttributeSyntax(_syntaxt, compilation)).ToList();
        }
    }
}
