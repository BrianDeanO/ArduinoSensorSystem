using backEndApp.DTO;
using backEndApp.Models;
using backEndApp.Data;
using backEndApp.Interfaces;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Data;

namespace backEndApp.Repository
{

    public class LocalDeviceRepository : IDeviceRepository
    {
        private static List<Device> _devices = new() { };
        private static int _lastDeviceId = 0;
        private IUserDeviceRepository _userDeviceRepository;

        public LocalDeviceRepository(IUserDeviceRepository userDeviceRepository)
        {
            _userDeviceRepository = userDeviceRepository;
        }

        // ICollections can only be read
        public ICollection<Device> GetDevices()
        {
            return _devices.OrderBy(d => d.DeviceID).ToList();
        }

        public Device? GetDevice(int deviceId)
        {
            return _devices.Where(d => d.DeviceID == deviceId).FirstOrDefault();
        }

        public ICollection<Device> GetDevicesForUser(int userId)
        {
            var deviceIds = _userDeviceRepository.GetUserDevices()
                .Where(ud => ud.UserID == userId)
                .Select(ud => ud.DeviceID)
                .ToList();
            return _devices.Where(d => deviceIds.Contains(d.DeviceID)).ToList();
        }

        public bool DeviceExists(int deviceId)
        {
            return _devices.Any(d => d.DeviceID == deviceId);
        }

        public bool CreateDevice(Device device)
        {
            device.DeviceID = _lastDeviceId++;
            if (_devices.Find(d => d.DeviceIdent == device.DeviceIdent) != null)
            {
                throw new DuplicateNameException("Device with ident '" + device.DeviceIdent + "' already exists");
            }
            _devices.Add(device);
            return true;
        }

        public bool UpdateDevice(Device device)
        {
            var old = _devices.Find(d => d.DeviceID == device.DeviceID);
            if (old == null)
            {
                return false;
            }

            _devices.Remove(old);
            _devices.Add(device);
            return true;
        }

        public bool DeleteDevice(Device device)
        {
            return _devices.Remove(device);
        }

        public bool Save()
        {
            return true;
        }
    }
}