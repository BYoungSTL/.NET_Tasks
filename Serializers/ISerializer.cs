using System.Collections.Generic;
using System.Threading.Tasks;
using NET02._2.Entities;

namespace NET02._2.Serializers
{
    public interface ISerializer<T>
    {
        public Task Serialize(List<T> logins);
        public Task<List<T>> Deserialize();
    }
}