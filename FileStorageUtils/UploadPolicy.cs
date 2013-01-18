using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FileStorageUtils
{
    [DataContract]
    class UploadPolicy
    {
        private List<IEnumerable<string>> _conditions;
        private DateTime _expiration;

        public UploadPolicy(DateTime expirationDate)
        {
            _expiration = expirationDate;
        }

        [DataMember(Name = "conditions")]
        public IEnumerable<IEnumerable<string>> Conditions
        {
            get { return _conditions; }

            private set { _conditions = new List<IEnumerable<string>>(value); }
        }

        [DataMember(Name = "expiration")]
        public string Expiration
        {
            get { return _expiration.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"); }
            private set { _expiration = DateTime.Parse(value); }
        }

        public void AddCondition(IEnumerable<string> condition)
        {
            if (null == _conditions)
                _conditions = new List<IEnumerable<string>>();

            _conditions.Add(condition);
        }
    }
}