using backEndApp.DTO;
using backEndApp.Models;
using backEndApp.Data;
using backEndApp.Interfaces;

namespace backEndApp.Repository {

    public class DeviceRepository: IDeviceRepository {
        private readonly SensorSystemContext _context;

        public DeviceRepository(SensorSystemContext context) {
            _context = context;
        }

        public ICollection<Device> GetDevices() {
            return _context.Devices.OrderBy(d => d.DeviceID).ToList();
        }

        public Device? GetDevice(int deviceId) {
            return _context.Devices.Where(d => d.DeviceID == deviceId).FirstOrDefault();
        }

        public ICollection<Sensor> GetDeviceSensors(int deviceId) {
            // var sensorList = _context.Sensors.Where(s => (s.DeviceID == deviceId)).ToList();
            return _context.Sensors.Where(s => (s.DeviceID == deviceId)).ToList();
        }

        public ICollection<Device> GetDevicesForUser(int userId) {
            var deviceIds = _context.UserDevices
                .Where(ud => ud.UserID == userId)
                .Select(ud => ud.DeviceID)
                .ToList();
            return _context.Devices.Where(d => deviceIds.Contains(d.DeviceID)).ToList();
        }


        public bool DeviceExists(int deviceId) {
            return _context.Devices.Any(d => d.DeviceID == deviceId); 
        }

        public bool CreateDevice(Device device) {
            _context.Add(device);
            return Save();
        }
        
        public bool UpdateDevice(Device device) {
            _context.Update(device);
            return Save();
        }

        public bool DeleteDevice(Device device) {
            _context.Remove(device);
            return Save();
        }

        public bool Save() {
            var saved = _context.SaveChanges();
            return (saved  > 0) ? true : false;
        }
    }
}