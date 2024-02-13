export interface DeviceType {
    deviceID: number;
    deviceName: string;
    deviceType: string;
    zipCode: string;
}

export interface DeviceDTOType {
    deviceName: string;
    deviceType: string;
    zipCode: string;
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
    // userUserName: string;
    userFirstName: string;
    userLastName: string;
    userType: string;  
    userPassword: string;
    userEmail: string;
    userPhone: string;
    userNotification: boolean;
}

export interface SensorType {
    sensorID: number;
    sensorName: string;
    sensorType: string;
    channelCount: number;
    deviceID: number;
}

export interface SensorDTOType {
    sensorName: string;
    sensorType: string;
    channelCount: number;
    deviceID: number;
}

export interface SensorDataType {
    // sensorDataID: number;
    channelID: number;
    dataValue: number;
    dataUnit: string;
    timeRecorded: string;
    sensorID: number;
}

export interface SelectedSensorData {
    selectedSensorID: number;
    // sensorData: SensorDataType[]
}

export interface SelectedTimeFrame {
    selectedTimeFrame: string;
}

export interface SelectedChannel {
    selectedChannelID: number;
}

export interface UserDeviceType {
    userID: number;
    deviceID: number;
    user: userType;
    device: deviceType;
}