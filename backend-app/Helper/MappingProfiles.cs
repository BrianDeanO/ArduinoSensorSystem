using AutoMapper;
using backEndApp.DTO;
using backEndApp.Models;

namespace backEndApp.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Device, DeviceDTO>();
            CreateMap<User, UserDTO>();
            CreateMap<Sensor, SensorDTO>();
            CreateMap<SensorData, SensorDataDTO>();
            CreateMap<SensorConfig, SensorConfigDTO>();
            CreateMap<UserDevice, UserDeviceDTO>();

            CreateMap<DeviceDTO, Device>();
            CreateMap<UserDTO, User>();
            CreateMap<SensorDTO, Sensor>();
            CreateMap<SensorWithConfigDTO, Sensor>();
            CreateMap<SensorDataDTO, SensorData>();
            CreateMap<SensorConfigDTO, SensorConfig>();
            CreateMap<UserDeviceDTO, UserDevice>();
        }
    }
}