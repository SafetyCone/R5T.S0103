using System;


namespace R5T.S0103
{
    public class Experiments : IExperiments
    {
        #region Infrastructure

        public static IExperiments Instance { get; } = new Experiments();


        private Experiments()
        {
        }

        #endregion
    }
}
