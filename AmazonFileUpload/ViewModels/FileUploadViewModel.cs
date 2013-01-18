using System;
using System.Security.Cryptography;
using System.Text;

namespace AmazonFileUpload.ViewModels
{
    public class FileUploadViewModel
    {
        private readonly string _privateKey;

        public FileUploadViewModel(string publicKey, string privateKey, string bucketName, string redirectUrl)
        {
            _privateKey = privateKey;

            // Document that describes the POST form upload metadata for Amazon S3
            // http://doc.s3.amazonaws.com/proposals/post.html#The_form_declaration

            FormAction = "http://" + bucketName + ".s3.amazonaws.com/";
            FormMethod = "post";
            FormEnclosureType = "multipart/form-data";

            Bucket = bucketName;
            FileId = Guid.NewGuid().ToString(); // Every file is uploaded with a new Guid as its ID
            AWSAccessKey = publicKey;
            Acl = "private"; // one of private, public-read, public-read-write, or authenticated-read
            RedirectUrl = redirectUrl;
        }

        public string FormAction { get; private set; }
        public string FormMethod { get; private set; } // "post"
        public string FormEnclosureType { get; private set; } // "multipart/form-data"
        public string Bucket { get; private set; }
        public string FileId { get; private set; }
        public string AWSAccessKey { get; private set; }
        public string Acl { get; private set; }
        public string Policy { get; private set; }
        public string RedirectUrl { get; private set; }

        public string Base64EncodedPolicy
        {
            get
            {
                var encoding = new ASCIIEncoding();
                var base64Policy = Convert.ToBase64String(encoding.GetBytes(Policy));
                return base64Policy;
            }

            private set { }
        }

        public string Signature
        {
            get
            {
                var encoding = new ASCIIEncoding();
                var hmacsha1 = new HMACSHA1(encoding.GetBytes(_privateKey));
                byte[] cipher = hmacsha1.ComputeHash(encoding.GetBytes(Base64EncodedPolicy));
                return Convert.ToBase64String(cipher);
            }

            private set { }
        }

        internal void SetPolicy(string policy)
        {
            Policy = policy;
        }
    }
}