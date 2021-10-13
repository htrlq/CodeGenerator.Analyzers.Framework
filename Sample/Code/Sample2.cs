namespace Sample
{
    /// <summary>
    /// Sample2
    /// </summary>
    internal class Sample2
    {
        /// <summary>
        /// internalMethod
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="left"></param>
        /// <returns></returns>
        internal string internalMethod(int arg, int left)
        {
            return string.Empty;
        }

        /// <summary>
        /// Show
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="left"></param>
        protected void Show(int arg, int left)
        {

        }

        /// <summary>
        /// Show1
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="left"></param>
        public virtual void Show1(int arg, int left)
        {

        }
    }

    //public class SamplePlug : IAnalyzer
    //{
    //    public void Execute(ProjectContext projectContext)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
