using DrTech.Models;
using DrTech.Models.ViewModels;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DrTech.DAL
{
    public interface IMongoDAL
    {
        Task InsertOne<TModel>(TModel model, string CollectionName);
        Task<TModel> FindOneByID<TModel>(string ID, string CollectionName);
        List<TModel> GetModelByUserID<TModel>(string UserID, string CollectionName);
        List<TModel> GetModelData<TModel>(List<FilterHelper> FilterValues, string CollectionName);
        List<TModel> GetModelData<TModel>(string CollectionName);
        Task AddSubDocument<TModel, SModel>(string parentId, SModel newSubCategory, string CollectionName, string subCollectionName);
        Task AddSubDocument<TModel, SModel>(FilterDefinition<TModel> filter, SModel newSubCategory, string CollectionName, string subCollectionName);
        long UpdateUserProfile ( UserViewModel mdlUser, string UserId);

        long UpdateStatusOfRecycleItems(MrCleanViewModel mdlClean);

        long UpdateUserGreenPoints(double GreenPoints, string UserId);
        bool UpdateStatus<TModel>(string id, UpdateDefinition<TModel> update, string collectionName);
        Task<bool> UpdateSubDocumentStatus<TParent, TChild>(string childId, int status, string parentCollection, string childCollection);

      //  Task<List<TParent>> GetAllSubDocuments<TParent, TChild>(string parentCollection, string childCollection, FilterDefinition<TChild> filter = null);

        Task<bool> UpdateSubDocument<TParent, TChild>(string childId, UpdateDefinition<TParent> update, string parentCollection, string childCollection);

        Task<List<TModel>> GetAllSubDocumentValueIfNotEmpty<TModel, SModel>(string field, string value,string collectionName, string subCollectionName);

        List<TModel> FullTextSearch<TModel>(string SearchWord, string CollectionName);


        Task<List<TModel>> GetAllSubDocuments<TModel, TChild>(string parentCollection, string childCollection, FilterDefinition<TChild> filter = null);
    }
}
