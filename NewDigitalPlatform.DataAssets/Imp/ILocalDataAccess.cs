
using System.Data;

namespace NewDigitalPlatform.DataAssets.Imp
{
    public interface ILocalDataAccess
    {
        DataTable Login(string username, string password);
       /* int SaveDevice(List<DeviceEntity> devices);
        List<DeviceEntity> GetDevices();


        List<PropEntity> GetPropertyOption();
        List<ThumbEntity> GetThumbList();*/
    }

}
