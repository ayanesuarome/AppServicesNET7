namespace CodeAnalyzing
{
    /// <summary>
    /// Widget class.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Using File-local type")]
    public class Widget2 : IWidget
    {
        /// <summary>
        /// Doc.
        /// </summary>
        /// <returns>Int.</returns>
        public int ProvideAnswer()
        {
            var worker = new HiddenWidget();
            return worker.Work();
        }
    }

    /// <summary>
    /// Interface.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:Elements should appear in the correct order", Justification = "Using File-local type")]
    file interface IWidget
    {
        /// <summary>
        /// Doc.
        /// </summary>
        /// <returns>Int.</returns>
        int ProvideAnswer();
    }

    /// <summary>
    /// Class.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1400:Access modifier should be declared", Justification = "Using File-local type")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Using File-local type")]
    file class HiddenWidget
    {
        /// <summary>
        /// Doc.
        /// </summary>
        /// <returns>Int.</returns>
        public int Work() => 42;
    }
}
