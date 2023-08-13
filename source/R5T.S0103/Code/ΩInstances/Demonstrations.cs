using System;


namespace R5T.S0103
{
    public class Demonstrations : IDemonstrations
    {
        #region Infrastructure

        public static IDemonstrations Instance { get; } = new Demonstrations();


        private Demonstrations()
        {
        }

        #endregion
    }
}
