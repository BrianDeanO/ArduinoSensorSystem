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

        // ICollections can only be read
        public ICollection<Device> GetDevices() {
            return _context.Devices.OrderBy(d => d.DeviceID).ToList();
        }

        public Device GetDevice(int deviceId) {
            return _context.Devices.Where(d => d.DeviceID == deviceId).FirstOrDefault();
        }

        public Device GetDeviceWithName(string deviceName) {
            return _context.Devices.Where(d => d.DeviceName == deviceName).FirstOrDefault();
        }

        public ICollection<Sensor> GetDeviceSensors(int deviceId) {
            // var sensorList = _context.Sensors.Where(s => (s.DeviceID == deviceId)).ToList();
            return _context.Sensors.Where(s => (s.DeviceID == deviceId)).ToList();
        }

        public ICollection<UserDevice> GetUserDevices(int deviceId) {
            // var sensorList = _context.Sensors.Where(s => (s.DeviceID == deviceId)).ToList();
            return _context.UserDevices.Where(ud => (ud.DeviceID == deviceId)).ToList();
        }

        public ICollection<User> GetUsersFromDevice(int deviceId) {
            // var sensorList = _context.Sensors.Where(s => (s.DeviceID == deviceId)).ToList();
            return _context.UserDevices.Where(ud => ud.DeviceID == deviceId).Select(ud => ud.User).ToList();
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