using Microsoft.CodeAnalysis;

namespace SampleAnalyzer
{
    static class Descriptors
    {
        public static DiagnosticDescriptor CompileUnitDescriptor { get; }
           = Create("P1001", "Project", "未找到源文件");

        public static DiagnosticDescriptor AnalyzerDescriptor { get; }
            = Create("A1001", "Analyzer", "解析失败");

        /// <summary>
        /// 创建诊断描述器
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="level"></param>
        /// <param name="helpLinkUri"></param>
        /// <returns></returns>
        private static DiagnosticDescriptor Create(string id, string title, string message, DiagnosticSeverity level = DiagnosticSeverity.Error)
        {
            var category = level.ToString();
            return new DiagnosticDescriptor(id, title, message, category, level, true, null);
        }
    }
}
