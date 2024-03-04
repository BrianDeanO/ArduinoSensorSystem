export interface DeviceType {
    deviceID: number;
    deviceIdent: string;
    deviceName: string;
    deviceType: string;
    deviceZipCode: string;
    devicePollingInterval: string;
}

export interface DeviceDTOType {
    deviceName: string;
    deviceType: string;
    deviceZipCode: string;
    devicePollingInterval: string;
}

export interface UserDeviceSelectedType {
    deviceID: number;
    isSelected: boolean;
}

export interface UserDeviceType {
    userID: number;
    deviceID: number;
}

// export interface UserDeviceType {
//     userID: number;
//     deviceID: number;
//     user: userType;
//     device: deviceType;
// }

// export interface PollingTimeIntervals {
//     SEC: string,
//     // 'MIN': string,
//     // 'HOUR': string,
//     // 'DAY': string,
//     // 'WEEK': string,
//     // 'MONTH': string,
// }

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
    userNotification: boolean;
}

export interface SensorType {
    sensorID: number;
    sensorIdent: string;
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
    sensorDataID: number;
    channelID: number;
    dataValue: number;
    dataUnit: string;
    timeRecorded: string;
    sensorID: number;
}

export interface SensorChannels {
    sensorID: number;
    channels: number[]
}

export interface SelectedSensorData {
    selectedSensorID: number;
    // sensorData: SensorDataType[]
}

export interface SelectedTimeFrame {
    selectedTimeFrame: number;
}

export interface SelectedChannel {
    selectedChannelID: number;
}

