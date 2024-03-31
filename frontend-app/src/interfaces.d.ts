export interface DeviceType {
    deviceID: number;
    deviceIdent: string;
    deviceName: string;
    deviceType: string;
    deviceZipCode: string;
    deviceUpdateInterval: number;
    deviceLastSeen: string;
    deviceIsDeleted: boolean;
}

export interface DeviceDTOType {
    deviceName: string;
    deviceType: string;
    deviceZipCode: string;
    deviceUpdateInterval: number;
    deviceLastSeen: string;
    deviceIsDeleted: boolean;
}

export interface UserDeviceSelectedType {
    deviceID: number;
    isSelected: boolean;
}

export interface UserDeviceType {
    userID: number;
    deviceID: number;
}
export interface SelectedDevice {
    selectedDeviceID: number;
}

export interface SelectedSensor {
    selectedSensorID: number;
}

export interface CurrentUserType {
    currentUserID: number;
    currentFirstName: string;
    currentLastName: string;
    isAdmin: boolean;
}

export interface UserType {
    userID: number;
    userFirstName: string;
    userLastName: string;
    userType: string;  
    userPassword: string;
    userEmail: string;
    userPhone: string;
    userNotifications: boolean;
    userIsDeleted: boolean;
}

export interface SensorType {
    sensorID: number;
    sensorIdent: string;
    sensorName: string;
    sensorType: string;
    channelCount: number;
    deviceID: number;
    sensorIsDeleted: boolean;
}

export interface SensorDTOType {
    sensorName: string;
    sensorType: string;
    channelCount: number;
    deviceID: number;
    sensorIsDeleted: boolean;
}

export interface SensorDataType {
    sensorDataID: number;
    channelID: number;
    dataValue: number;
    dataUnit: string;
    timeRecorded: string;
    sensorID: number;
}

export interface SensorConfigType {
    sensorConfigID: number;
    sensorID: number;
    sensorConfigKey: string;
    sensorConfigValue: string;
}

export interface SensorChannels {
    sensorID: number;
    channels: number[]
}

export interface SelectedSensorData {
    selectedSensorID: number;
}

export interface SelectedTimeFrame {
    selectedTimeFrame: number;
}

export interface SelectedChannel {
    selectedChannelID: number;
}

