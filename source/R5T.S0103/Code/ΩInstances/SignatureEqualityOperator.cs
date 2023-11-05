using System;


namespace R5T.S0103
{
    public class SignatureEqualityOperator : ISignatureEqualityOperator
    {
        #region Infrastructure

        public static ISignatureEqualityOperator Instance { get; } = new SignatureEqualityOperator();


        private SignatureEqualityOperator()
        {
        }

        #endregion
    }
}
