using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DrTech.ExceptionLogger
{
    [BsonDiscriminator("ExceptionLogging")]
    public class ExceptionLogging
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Date { get; set; }
        public string ErrorMessage { get; set; }
        public string InnerErrorMessage { get; set; }
        public string StackTrace { get; set; }
    }
}