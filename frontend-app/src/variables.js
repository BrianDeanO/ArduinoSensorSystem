export const version = "1.0";
export const proxyURL_HTTPS = "https://localhost:8443"; // 7239 before
export const proxyURL = "http://localhost:8080"; // 5270 before
export const ADMIN = 'ADMIN';
export const BASIC = 'BASIC';

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

export const timeFrameConstants = {
    DAY: 1000 * 60 * 60 * 24,
    WEEK: 1000 * 60 * 60 * 24 * 7,
    MONTH: (1000 * 60 * 60 * 24 * 7 * 4) + 2,
    SIX_MONTHS: (1000 * 60 * 60 * 24 * 7 * 4 + 2) * 6,
    YEAR: 1000 * 60 * 60 * 24 * 7 * 52,
}