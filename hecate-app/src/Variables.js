export const version = "1.0";
export const proxyURL_HTTPS = "https://localhost:8443"; // 7239 before
export const proxyURL = "http://localhost:8080"; // 5270 before
export const ADMIN = 'ADMIN';
export const BASIC = 'BASIC';
export const channelCountArray = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

export const repeatCountLimits = {
    DAY: 2,
    WEEK_MONTH: 4,
    HALF_AND_FULL_YEAR: 5,
    LIFETIME: 12
}

export const monthsOfTheYear = [
    'Jan',
    'Feb',
    'March',
    'April',
    'May',
    'June',
    'July',
    'Aug',
    'Sep',
    'Oct',
    'Nov',
    'Dec'
]

export const localStorageTitles = {
    currentUser: "currentUser",
    selectedDevice: "selectedDevice",
    selectedSensor: "selectedSensor",
    selectedSensorData: "selectedSensorData",
    selectedTimeFrame: "selectedTimeFrame",
    selectedChannel: "selectedChannel"
}

export const pollingTimeIntervalLabels = {
    SEC: `Second(s)`,
    MIN: `Minute(s)`,
    HOUR: `Hour(s)`,
    DAY: `Day(s)`,
    WEEK: `Week(s)`,
    MONTH: `Month(s)`,
}

export const timeFrameConstants = {
    DAY: 1000 * 60 * 60 * 24,
    WEEK: 1000 * 60 * 60 * 24 * 7,
    // MONTH: (1000 * 60 * 60 * 24 * 7 * 4) + (1000 * 60 * 60 * 24 * 2),
    MONTH: (1000 * 60 * 60 * 24 * 7 * 4) + 2,
    SIX_MONTHS: (1000 * 60 * 60 * 24 * 7 * 4 + 2) * 6,
    YEAR: 1000 * 60 * 60 * 24 * 7 * 52,
}

export const apiInfo = {
    USERS: `${proxyURL}/api/Users`,
    DEVICES: `${proxyURL}/api/Devices`,
    USER_DEVICES: `${proxyURL}/api/UserDevices`,
    SENSORS: `${proxyURL}/api/Sensors`,
    DEVICE_SENSORS: `${proxyURL}/api/DeviceSensors`,
    SENSOR_DATA: `${proxyURL}/api/SensorDatas`,
}

export const apiInfo2 = {
    USERS: {
        URL: `${proxyURL}/api/Users`,
        INDEX: 1
    },
    DEVICES: {
        URL: `${proxyURL}/api/Devices`,
        INDEX: 2
    },
    USER_DEVICES: {
        URL: `${proxyURL}/api/UserDevices`,
        INDEX: 3
    },
    SENSORS: {
        URL: `${proxyURL}/api/Sensors`,
        INDEX: 4
    },
    DEVICE_SENSORS: {
        URL: `${proxyURL}/api/DeviceSensors`,
        INDEX: 5
    },
    SENSOR_DATA: {
        URL: `${proxyURL}/api/SensorDatas`,
        INDEX: 6
    },
}

export const dataObject = (temp, unit, time) => {
    return {
        Temperature: temp,
        Unit: unit,
        Time: time
    }
}

export const userTable = [
    {
        user_ID: 1, 
        user_type: "admin", 
        user_name: "Han", 
        user_password: "123", 
        user_email: "",
        user_phone: "",
        user_notifications: true
    },
    {
        user_ID: 2,
        user_type: "basic", 
        user_name: "Leia", 
        user_password: "456", 
        user_email: "",
        user_phone: "",
        user_notifications: false
    },
    {
        user_ID: 3, 
        user_type: "basic", 
        user_name: "Luke", 
        user_password: "789", 
        user_email: "",
        user_phone: "",
        user_notifications: true
    },
]

export const deviceTable = [
    {deviceID: 1, deviceIdent: 'ARD-123', deviceName: "RICHLAND_NW_1", deviceType: "ARDUINO", deviceZipCode: '99352'},
    {deviceID: 2, deviceIdent: 'ARD-456', deviceName: "KENNEWICK_N_1", deviceType: "ARDUINO", deviceZipCode: '99336'},
    {deviceID: 3, deviceIdent: 'ARD-789', deviceName: "PASCO_E_1", deviceType: "ARDUINO", deviceZipCode: '99301'}
]

export const userDeviceEntriesTable = [
    {user_ID: 1, deviceID: 1},
    {user_ID: 1, deviceID: 2},
    {user_ID: 1, deviceID: 3},
    {user_ID: 2, deviceID: 1},
    {user_ID: 3, deviceID: 1},
    {user_ID: 3, deviceID: 2}
]

export const deviceSensorEntriesTable = [
    {deviceID: 1, sensorID: 1},
    {deviceID: 1, sensorID: 2},
    {deviceID: 2, sensorID: 3},
    {deviceID: 3, sensorID: 4},
]

export const sensorTable = [
    {sensorID: 1, sensorIdent: 'SEN-123', sensorName: "TEMP_1", sensorType: "Temperature", channelCount: 2},
    {sensorID: 2, sensorIdent: 'SEN-456', sensorName: "IRRAD_1", sensorType: "Irradiance", channelCount: 1},
    {sensorID: 3, sensorIdent: 'SEN-789', sensorName: "PRESSURE_1", sensorType: "Pressure", channelCount: 2},
    {sensorID: 4, sensorIdent: 'SEN-123-0', sensorName: "TEMP_2", sensorType: "Temperature", channelCount: 2},
]

export const sensorDataTable = [
    {sensorDataID: 1, sensorID: 1, channelID: 1, dataValue: 13, dataUnit: "F", timeRecorded: "08:00"},
    {sensorDataID: 2, sensorID: 1, channelID: 1, dataValue: 14, dataUnit: "F", timeRecorded: "09:00"},
    {sensorDataID: 3, sensorID: 1, channelID: 2, dataValue: 15, dataUnit: "F", timeRecorded: "10:00"},
    {sensorDataID: 4, sensorID: 1, channelID: 2, dataValue: 12, dataUnit: "F", timeRecorded: "11:00"},
    {sensorDataID: 5, sensorID: 1, channelID: 2, dataValue: 16, dataUnit: "F", timeRecorded: "12:00"},
    {sensorDataID: 6, sensorID: 1, channelID: 1, dataValue: 19, dataUnit: "F", timeRecorded: "13:00"},
    {sensorDataID: 7, sensorID: 2, channelID: 1, dataValue: 1361, dataUnit: "W/m^2", timeRecorded: "14:00"},
    {sensorDataID: 8, sensorID: 2, channelID: 1, dataValue: 1459, dataUnit: "W/m^2", timeRecorded: "15:00"},
    {sensorDataID: 9, sensorID: 3, channelID: 1, dataValue: 30, dataUnit: "atm", timeRecorded: "16:00"},
    {sensorDataID: 10, sensorID: 3, channelID: 2, dataValue: 31, dataUnit: "atm", timeRecorded: "17:00"},
    {sensorDataID: 11, sensorID: 3, channelID: 1, dataValue: 33, dataUnit: "atm", timeRecorded: "18:00"},
    {sensorDataID: 12, sensorID: 3, channelID: 2, dataValue: 29, dataUnit: "atm", timeRecorded: "19:00"},
    {sensorDataID: 13, sensorID: 3, channelID: 1, dataValue: 31, dataUnit: "atm", timeRecorded: "20:00"}
]

export const sensorDataTable2 = [
    {
        channelID: 1, 
        dataValue: 13, 
        dataUnit: "F", 
        timeRecorded: "2024-01-01T18:42:27.069Z",
        sensorID: 1
    },
    {
        channelID: 1, 
        dataValue: 14, 
        dataUnit: "F", 
        timeRecorded: "2024-01-02T18:42:27.069Z",
        sensorID: 1
    },
    { 
        channelID: 2, 
        dataValue: 15, 
        dataUnit: "F", 
        timeRecorded: "2024-01-03T18:42:27.069Z",
        sensorID: 1
    },
    {
        channelID: 2, 
        dataValue: 12, 
        dataUnit: "F", 
        timeRecorded: "2024-01-04T18:42:27.069Z",
        sensorID: 1
    },
    { 
        channelID: 2, 
        dataValue: 16, 
        dataUnit: "F", 
        timeRecorded: "2024-01-05T18:42:27.069Z",
        sensorID: 1
    },
    {
        channelID: 1, 
        dataValue: 19, 
        dataUnit: "F", 
        timeRecorded: "2024-01-06T18:42:27.069Z",
        sensorID: 1
    },
    { 
        channelID: 1, 
        dataValue: 1361, 
        dataUnit: "W/m^2", 
        timeRecorded: "2024-01-07T18:42:27.069Z",
        sensorID: 2
    },
    { 
        channelID: 1, 
        dataValue: 1459,
        dataUnit: "W/m^2",
        timeRecorded: "2024-01-08T18:42:27.069Z",
        sensorID: 2
    },
    { 
        channelID: 1, 
        dataValue: 30,
        dataUnit: "atm",
        timeRecorded: "2024-01-09T18:42:27.069Z",
        sensorID: 3
    },
    { 
        channelID: 2, 
        dataValue: 31,
        dataUnit: "atm", 
        timeRecorded: "2024-01-10T18:42:27.069Z",
        sensorID: 3
    },
    { 
        channelID: 1, 
        dataValue: 33,
        dataUnit: "atm",
        timeRecorded: "2024-01-11T18:42:27.069Z",
        sensorID: 3
    },
    { 
        channelID: 2, 
        dataValue: 29,
        dataUnit: "atm", 
        timeRecorded: "2024-01-12T18:42:27.069Z",
        sensorID: 3
    },
    { 
        channelID: 1, 
        dataValue: 31, 
        dataUnit: "atm", 
        timeRecorded: "2024-01-13T18:42:27.069Z",
        sensorID: 3
    }
]