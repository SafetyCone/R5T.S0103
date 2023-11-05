using System;
using System.Collections.Generic;

using R5T.L0062.T000;
using R5T.L0063.T000;
using R5T.T0172;


namespace R5T.S0103
{
    public class ProjectFileResult
    {
        public IProjectFilePath ProjectFilePath { get; set; }
        public List<Exception> Exceptions { get; } = new();
        public List<IdentityStringPair> IdentityString_Failures { get; } = new();
        public List<IdentityStringPair> IdentityString_Successes { get; } = new();
        public List<SignatureStringPair> SignatureString_Failures { get; } = new();
        public List<SignatureStringPair> SignatureString_Successes { get; } = new();
    }

    public class IdentityStringPair
    {
        public IIdentityString IdentityString { get; set; }
        public IIdentityString SignatureIdentityString { get; set; }
    }

    public class SignatureStringPair
    {
        public IIdentityString IdentityString { get; set; }
        public ISignatureString SignatureString { get; set; }
    }
}
