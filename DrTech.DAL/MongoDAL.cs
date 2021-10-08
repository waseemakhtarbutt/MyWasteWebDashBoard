//using Microsoft.Extensions.Configuration;
using DrTech.Common.Helpers;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrTech.Common.Extentions;
using MongoDB.Driver.Linq;
using DrTech.Models;
using System;
using DrTech.Models.ViewModels;


namespace DrTech.DAL
{
    public class MongoDAL : IMongoDAL
    {
        IMongoDatabase db;

        public MongoDAL()
        {
            string ConnectionString = AppSettingsHelper.GetAttributeValue(Constants.AppSettings.MONGO_SECTION, Constants.AppSettings.MONGO_CONSTR);
            string Database = AppSettingsHelper.GetAttributeValue(Constants.AppSettings.MONGO_SECTION, Constants.AppSettings.MONGO_DB);

            //var pack = new ConventionPack
            //{
            //    new IgnoreExtraElementsConvention(true)
            //};
            //ConventionRegistry.Register("My Solution Conventions", pack, t => true);

            var client = new MongoClient(ConnectionString);
            db = client.GetDatabase(Database);
        }

        public MongoDAL(string ConnectionString)
        {
            //string ConnectionString = AppSettingsHelper.GetAttributeValue(Constants.AppSettings.MONGO_SECTION, Constants.AppSettings.MONGO_CONSTR);
            string Database = AppSettingsHelper.GetAttributeValue(Constants.AppSettings.MONGO_SECTION, Constants.AppSettings.MONGO_DB);

            //var pack = new ConventionPack
            //{
            //    new IgnoreExtraElementsConvention(true)
            //};
            //ConventionRegistry.Register("My Solution Conventions", pack, t => true);

            var client = new MongoClient(ConnectionString);
            db = client.GetDatabase(Database + "_LiveData");
        }


        public async Task InsertOne<TModel>(TModel model, string CollectionName)
        {
            var collection = db.GetCollection<TModel>(CollectionName);
            await collection.InsertOneAsync(model);
        }

        public void InsertOneSync<TModel>(TModel model, string CollectionName)
        {
            var collection = db.GetCollection<TModel>(CollectionName);
            collection.InsertOneAsync(model);
        }

        public void InsertManySync<TModel>(List<TModel> lstModels, string CollectionName)
        {
            var collection = db.GetCollection<TModel>(CollectionName);
            collection.InsertMany(lstModels);
        }


        public async Task<TModel> FindOneByID<TModel>(string ID, string CollectionName)
        {
            var dd = await db.GetCollection<TModel>(CollectionName).Find(new BsonDocument { { "_id", new ObjectId(ID) } })?.FirstOrDefaultAsync();

            return dd;
        }

        public TModel FindOneByIDSync<TModel>(string ID, string CollectionName)
        {
            var collection = db.GetCollection<TModel>(CollectionName);
            var filter = Builders<TModel>.Filter.Eq("_id", ObjectId.Parse(ID));
            return collection.FindSync<TModel>(filter).FirstOrDefault();
        }

        public List<TModel> FullTextSearch<TModel>(string SearchWord, string CollectionName)
        {
            var keys = Builders<TModel>.IndexKeys.Text("Name");
            var collection = db.GetCollection<TModel>(CollectionName);
            collection.Indexes.CreateOne(keys);
            var filter = Builders<TModel>.Filter.Text(SearchWord);
            var result = collection.Find(filter).ToList();
            return result;
        }

        public List<TModel> GetModelByUserID<TModel>(string UserID, string CollectionName)
        {
            var collection = db.GetCollection<TModel>(CollectionName);
            var filter = Builders<TModel>.Filter.Eq("UserId", UserID);

            return collection.FindSync<TModel>(filter)?.ToList();
        }

        public List<TModel> GetModelData<TModel>(List<FilterHelper> FilterValues, string CollectionName)
        {
            var collection = db.GetCollection<TModel>(CollectionName);

            var builder = Builders<TModel>.Filter;

            FilterDefinition<TModel> filters = null;

            foreach (FilterHelper filter in FilterValues)
            {
                FilterDefinition<TModel> newfilter = builder.Eq(filter.Field, filter.Value);
                filters = (filters == null) ? newfilter : filters & newfilter;
            }

            return collection.FindSync<TModel>(filters)?.ToList();
        }

        public async Task<List<TModel>> GetAllSubDocumentValueIfNotEmpty<TModel, SModel>(string field, string value, string collectionName, string subCollectionName)
        {
            //var collection = db.GetCollection<Users>(collectionName);

            //var filters = Builders<TModel>.Filter.Size(field, 0);   
            //var collection = db.GetCollection<Users>(collectionName);

            //var filters = Builders<TModel>.Filter.Size(field, 0);

            //var result = collection
            //                    .Aggregate()
            //                    .Project(c => new
            //                    {
            //                        sub = c.Donation.Where(a => !string.IsNullOrEmpty(a.NeedType ))
            //                    })
            //                    .ToList();

            var filter = Builders<TModel>.Filter.ElemMatch(subCollectionName, Builders<SModel>.Filter.Eq(field, value));

            //var result12 = await collection.Find(filter).ToListAsync();
            //var vv = collection.FindSync<TModel>(filters).ToList();
            return null;
        }

        public List<TModel> GetModelData<TModel>(string CollectionName)
        {
            var collection = db.GetCollection<TModel>(CollectionName);
            FilterDefinition<TModel> filter = FilterDefinition<TModel>.Empty;

            return collection.FindSync<TModel>(filter)?.ToList();

        }
        public async Task AddSubDocument<TModel, SModel>(string parentId, SModel newSubCategory, string CollectionName, string subCollectionName)
        {
            var parentDocumentFilter = Builders<TModel>.Filter.And(new BsonDocument { { "_id", new ObjectId(parentId) } });
            await AddSubDocument(parentDocumentFilter, newSubCategory, CollectionName, subCollectionName);
        }
        public async Task AddSubDocument<TModel, SModel>(FilterDefinition<TModel> filter, SModel newSubCategory, string CollectionName, string subCollectionName)
        {
            var collection = db.GetCollection<TModel>(CollectionName);
            //   var dd = await collection.FindAsync(filter);
            var update = Builders<TModel>.Update;
            var courseLevelSetter = update.Push(subCollectionName, newSubCategory);
            await collection.UpdateOneAsync(filter, courseLevelSetter);
        }
        //public async Task GetSubDocuments<TModel, SModel>(string parentId, string CollectionName, string subCollectionName)
        //{
        //    var collection = db.GetCollection<TModel>(CollectionName);

        //    var filter = Builders<TModel>.Filter;
        //    var parentDocumentFilter = filter.And(new BsonDocument { { "_id", new ObjectId(parentId) } });
        //    var projection = Builders<TModel>.Projection;
        //    var courseLevelSetter = projection.As(subCollectionName);
        //    //collection.FindAsync(parentDocumentFilter, courseLevelSetter);
        //}

        //public async Task Update<TModel>(string parentId, TModel mdlModel, string CollectionName)
        //{
        //    //var filter = new BsonDocument();
        //    string FullName = CommonFunction.GetDynamicPropertyValue(mdlModel, "FullName");
        //    string Phone = CommonFunction.GetDynamicPropertyValue(mdlModel, "Phone");
        //    string Address = CommonFunction.GetDynamicPropertyValue(mdlModel, "Address");
        //    string Email = CommonFunction.GetDynamicPropertyValue(mdlModel, "Email");
        //    string Password = CommonFunction.GetDynamicPropertyValue(mdlModel, "Password");
        //    string FileName = CommonFunction.GetDynamicPropertyValue(mdlModel, "FileName");


        //    //string name =  Convert.ToString(mdlModel.GetType().GetProperty("FullName").GetValue(mdlModel, null));
        //    BsonDocument update = new BsonDocument();
        //    var filter = Builders<TModel>.Filter;
        //    var parentDocumentFilter = filter.And(new BsonDocument { { "_id", new ObjectId(parentId) } });
        //     var updates = new BsonDocument("$set", new BsonDocument("FullName", FullName));

        //    BsonDocument childBsonDoc = new BsonDocument();
        //    childBsonDoc.Add(new BsonElement("FullName", FullName));
        //    childBsonDoc.Add(new BsonElement("Phone", Phone));
        //    childBsonDoc.Add(new BsonElement("Password", Password));
        //    updates.AddRange(childBsonDoc);
        //    update.Add(new BsonElement("Users", updates));

        //    var options = new UpdateOptions { IsUpsert = true };
        //    var collection = db.GetCollection<TModel>("Users");
        //    // var result = collection.UpdateOne(parentDocumentFilter, updates, options);

        //    var update = Update.Set("Email", "jdoe@gmail.com")
        //            .Set("Phone", "4455512");
        //    //collection.UpdateOne(new BsonDocument("_id", "some_filter"), new BsonDocument("$set", new BsonDocument(update)), new UpdateOptions { IsUpsert = true });



        //    //var result = await collection.UpdateOneAsync(filter, update, options);
        //}


        public long UpdateUserProfile(UserViewModel mdlRegister, string UserID)
        {
            if (mdlRegister != null)
            {
                var collection = db.GetCollection<Users>(Constants.CollectionNames.USERS);
                var filter = Builders<Users>.Filter.Eq("_id", ObjectId.Parse(UserID));
                var user = FindOneByID<Users>(UserID, Constants.CollectionNames.USERS).Result;
                if (mdlRegister.FullName == "" || mdlRegister.FullName == null)
                    mdlRegister.FullName = user.FullName;
                if (mdlRegister.Email == "" || mdlRegister.Email == null)
                    mdlRegister.Email = user.Email;
                if (mdlRegister.Password == "" || mdlRegister.Password == null)
                    mdlRegister.Password = user.Password;
                if (mdlRegister.FileName == "" || mdlRegister.FileName == "")
                    mdlRegister.FileName = user.FileName;

                var update = Builders<Users>.Update
                    .Set(o => o.FullName, mdlRegister.FullName)
                    .Set(g => g.Email, mdlRegister.Email)
                    .Set(d => d.Password, mdlRegister.Password)
                    .Set(a => a.Address, mdlRegister.Address)
                    .Set(p => p.FileName, mdlRegister.FileName);

                UpdateResult ur = collection.UpdateOne(filter, update);
                if (ur.IsAcknowledged)
                    return ur.ModifiedCount;
                else
                    return 0;

            }

            return 0;
        }


        public long UpdateStatusOfRecycleItems(MrCleanViewModel mdlClean)
        {
            if (mdlClean != null)
            {
                string Id = Convert.ToString(mdlClean.Id);
                var collection = db.GetCollection<MrClean>(Constants.CollectionNames.RECYCLE);
                var filter = Builders<MrClean>.Filter.Eq("_id", ObjectId.Parse(Id));

                var update = Builders<MrClean>.Update
                    .Set(o => o.Status, mdlClean.Status)
                    .Set(g => g.CollectorDateTime, mdlClean.CollectorDateTime);

                UpdateResult ur = collection.UpdateOne(filter, update);
                if (ur.IsAcknowledged)
                    return ur.ModifiedCount;
                else
                    return 0;

            }

            return 0;
        }
        public bool UpdateStatus<TModel>(string id, UpdateDefinition<TModel> update, string collectionName)
        {
            string Id = Convert.ToString(id);
            var collection = db.GetCollection<TModel>(collectionName);
            var filter = Builders<TModel>.Filter.Eq("_id", ObjectId.Parse(Id));

            UpdateResult ur = collection.UpdateOne(filter, update);
            if (ur.IsAcknowledged && ur.ModifiedCount > 0)
                return true;
            else
                return false;
        }
        public async Task<bool> UpdateSubDocumentStatus<TParent, TChild>(string childId, int status, string parentCollection, string childCollection)
        {
            var collection = db.GetCollection<TParent>(parentCollection);

            var update = Builders<TParent>.Update.Set(childCollection + ".$.Status", status);

            var filter = Builders<TParent>.Filter.ElemMatch(childCollection, Builders<TChild>.Filter.And(new BsonDocument { { "_id", new ObjectId(childId) } }));
            //var result = await collection.Find(filter).ToListAsync();
            var result = await collection.FindOneAndUpdateAsync<TParent>(filter, update);

            return true;
        }

        public async Task<bool> UpdateSubDocument<TParent, TChild>(string childId, UpdateDefinition<TParent> update, string parentCollection, string childCollection)
        {
            var collection = db.GetCollection<TParent>(parentCollection);

            //var update = Builders<TParent>.Update.Set(childCollection + ".$.Status", "")
            //                                        .Set(childCollection + ".$.Status", "");

            var filter = Builders<TParent>.Filter.ElemMatch(childCollection, Builders<TChild>.Filter.And(new BsonDocument { { "_id", new ObjectId(childId) } }));
            var result12 = await collection.Find(filter).ToListAsync();

            var result = await collection.FindOneAndUpdateAsync<TParent>(filter, update);

            Users u = result12.Cast<Users>().ToList<Users>().FirstOrDefault();

            string Id = u.Id.ToString();

            Users user = FindOneByID<Users>(Id, Constants.CollectionNames.USERS).Result;


            
            int GP = Convert.ToInt32(user.GreenPoints);

            if (childCollection == Constants.CollectionNames.Refuse)
            {
                if (user?.Refuse?.Count > 0)
                {
                    foreach (var reduce in user?.Refuse)
                    {
                        if (reduce.Id.ToString() == childId)
                        {
                            GP += reduce.GreenPoints;
                        }
                    }
                }
            }


            if (childCollection == Constants.CollectionNames.Reduce)
            {
                if (user?.Reduce?.Count > 0)
                {
                    foreach (var reduce in user?.Reduce)
                    {
                        if (reduce.Id.ToString() == childId)
                        {
                            GP += reduce.GreenPoints;
                        }
                    }
                }
            }

            if (childCollection == Constants.CollectionNames.REPLANT)
            {
                if (user?.Replant?.Count > 0)
                {
                    foreach (var reduce in user?.Replant)
                    {
                        if (reduce.Id.ToString() == childId)
                        {
                            GP += reduce.GreenPoints;
                        }
                    }
                }
            }


            if (childCollection == Constants.CollectionNames.REUSE)
            {
                if (user?.Reuse?.Count > 0)
                {
                    foreach (var reduce in user?.Reuse)
                    {
                        if (reduce.Id.ToString() == childId)
                        {
                            GP += reduce.GreenPoints;
                        }
                    }
                }
            }


            long Count = UpdateUserGreenPoints(GP, Id);

            return true;
        }
        public long UpdateUserGreenPoints(Double GreenPoints, string UserID)
        {

            var collection = db.GetCollection<Users>(Constants.CollectionNames.USERS);
            var filter = Builders<Users>.Filter.Eq("_id", ObjectId.Parse(UserID));


            var update = Builders<Users>.Update
                .Set(o => o.GreenPoints, GreenPoints);

            UpdateResult ur = collection.UpdateOne(filter, update);
            if (ur.IsAcknowledged)
                return ur.ModifiedCount;
            else
                return 0;
        }

        public async Task<List<TModel>> GetAllSubDocuments<TModel, TChild>(string parentCollection, string childCollection, FilterDefinition<TChild> filters = null)
        {
            var collection = db.GetCollection<TModel>(parentCollection);
            FilterDefinition<TModel> filter = FilterDefinition<TModel>.Empty;

            return collection.FindSync<TModel>(filter)?.ToList();

            //var collection = db.GetCollection<TParent>(parentCollection);

            //if (filter != null)
            //{
            //    var filter1 = Builders<TParent>.Filter.ElemMatch(childCollection, filter);
            //    return await collection.Find(filter1).ToListAsync();
            //}
            //var filter2 = Builders<TParent>.Filter.ElemMatch(childCollection, Builders<TChild>.Filter.Empty);
            //return await collection.Find(filter2)?.ToListAsync();
        }


 

    }

}
