
using NewDigitalPlatform.Entities;
using System.Data;

namespace NewDigitalPlatform.DataAssets.Imp
{
    public interface ILocalDataAccess
    {
        DataTable Login(string username, string password);
        List<DeviceEntity> GetDevices();
        void SaveDevice(List<DeviceEntity> devices);



        List<PropEntity> GetPropertyOption();
        List<ThumbEntity> GetThumbList();
    }

}
