using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace FileStorageUtils
{
    public class AmazonS3FileStorageProvider
    {
        private const string PublicKeySetting = "AWSAccessKey";
        private const string PrivateKeySetting = "AWSSecretKey";
        private const string BucketNameSetting = "AWSBucket";

        public AmazonS3FileStorageProvider()
        {
            PublicKey = ConfigurationManager.AppSettings[PublicKeySetting];
            PrivateKey = ConfigurationManager.AppSettings[PrivateKeySetting];
            BucketName = ConfigurationManager.AppSettings[BucketNameSetting];
        }

        public string PublicKey { get; private set; }
        public string PrivateKey { get; private set; }
        public string BucketName { get; private set; }

        public string GetPolicyString(string fileId, string redirectUrl)
        {
            var policy = new UploadPolicy(DateTime.Now.AddHours(10));

            policy.AddCondition(new List<string> { "eq", "$bucket", BucketName });
            policy.AddCondition(new List<string> { "eq", "$acl", "private" });
            policy.AddCondition(new List<string> { "content-length-range", "0", "100000000" });
            policy.AddCondition(new List<string> { "eq", "$key", fileId });
            policy.AddCondition(new List<string> { "eq", "$redirect", redirectUrl });

            var ser = new DataContractJsonSerializer(typeof(UploadPolicy));
            var ms = new MemoryStream();
            ser.WriteObject(ms, policy);

            var json = Encoding.Default.GetString(ms.ToArray());

            return json;
        }
    }
}