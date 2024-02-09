export interface DeviceType {
    deviceID: number;
    deviceName: string;
    deviceType: string;
    zipCode: number;
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
}

export interface UserType {
    userID: number;
    userFirstName: string;
    userLastName: string;
    userType: string;  
    userEmail: string;
    userPhone: string;
    userNotification: string;
}

export interface SensorType {
    sensorID: number;
    sensorName: string;
    // sensorType: string;
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

export interface UserDeviceType {
    userID: number;
    deviceID: number;
    user: userType;
    device: deviceType;
}