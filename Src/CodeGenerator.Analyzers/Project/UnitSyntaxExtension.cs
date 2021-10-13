using CodeGenerator.Analyzers.Framework.Project.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenerator.Analyzers.Framework.Project
{
    internal static class UnitSyntaxExtension
    {
        public static List<UnitSyntax> RemoveNull(this List<UnitSyntax> unitSyntaxes)
        {
            return unitSyntaxes.Where(_syntax => _syntax.Classes != null).ToList();
        }
    }
}
