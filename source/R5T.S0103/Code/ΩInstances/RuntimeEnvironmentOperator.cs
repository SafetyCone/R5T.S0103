using System;


namespace R5T.S0103
{
    public class RuntimeEnvironmentOperator : IRuntimeEnvironmentOperator
    {
        #region Infrastructure

        public static IRuntimeEnvironmentOperator Instance { get; } = new RuntimeEnvironmentOperator();


        private RuntimeEnvironmentOperator()
        {
        }

        #endregion
    }
}


namespace R5T.S0103.Internal
{
    public class RuntimeEnvironmentOperator : IRuntimeEnvironmentOperator
    {
        #region Infrastructure

        public static IRuntimeEnvironmentOperator Instance { get; } = new RuntimeEnvironmentOperator();


        private RuntimeEnvironmentOperator()
        {
        }

        #endregion
    }
}
