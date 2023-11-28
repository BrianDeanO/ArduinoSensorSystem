export const version = '1.0';

export  const variables = {
    API_URL: 'http://localhost:7239/api/'
}

export const dataObject = (temp, unit, time) => {
    return {
        Temperature: temp,
        Unit: unit,
        Time: time
    }
}

export const userTable = [
    {user_ID: 1, user_type: 'admin', user_name: 'Han', user_password: '123', user_email: ''},
    {user_ID: 2, user_type: 'basic', user_name: 'Luke', user_password: '456', user_email: ''},
    {user_ID: 3, user_type: 'basic', user_name: 'Leia', user_password: '789', user_email: ''},
]

export const userSensorEntriesTable = [
    {user_ID: 1, sensor_ID: 1},
    {user_ID: 1, sensor_ID: 2},
    {user_ID: 1, sensor_ID: 3},
    {user_ID: 2, sensor_ID: 1},
    {user_ID: 3, sensor_ID: 1},
    {user_ID: 3, sensor_ID: 2}
]

export const sensorTable = [
    {sensor_ID: 1, sensor_name: 'TEMP_1', sensor_type: 'Temperature', channel_count: 2},
    {sensor_ID: 2, sensor_name: 'IRRAD_1', sensor_type: 'Irradiance', channel_count: 1},
    {sensor_ID: 3, sensor_name: 'PRESSURE_1', sensor_type: 'Pressure', channel_count: 2}
]

export const sensorDataTable = [
    {data_ID: 1, sensor_ID: 1, channel_ID: 1, data_value: 13, data_unit: 'F', time_recorded: '08:00'},
    {data_ID: 2, sensor_ID: 1, channel_ID: 1, data_value: 14, data_unit: 'F', time_recorded: '09:00'},
    {data_ID: 3, sensor_ID: 1, channel_ID: 2, data_value: 15, data_unit: 'F', time_recorded: '10:00'},
    {data_ID: 4, sensor_ID: 1, channel_ID: 2, data_value: 12, data_unit: 'F', time_recorded: '11:00'},
    {data_ID: 5, sensor_ID: 1, channel_ID: 2, data_value: 16, data_unit: 'F', time_recorded: '12:00'},
    {data_ID: 6, sensor_ID: 1, channel_ID: 1, data_value: 19, data_unit: 'F', time_recorded: '13:00'},
    {data_ID: 7, sensor_ID: 2, channel_ID: 1, data_value: 1361, data_unit: 'W/m^2', time_recorded: '14:00'},
    {data_ID: 8, sensor_ID: 2, channel_ID: 1, data_value: 1459, data_unit: 'W/m^2', time_recorded: '15:00'},
    {data_ID: 9, sensor_ID: 3, channel_ID: 1, data_value: 30, data_unit: 'atm', time_recorded: '16:00'},
    {data_ID: 10, sensor_ID: 3, channel_ID: 2, data_value: 31, data_unit: 'atm', time_recorded: '17:00'},
    {data_ID: 11, sensor_ID: 3, channel_ID: 1, data_value: 33, data_unit: 'atm', time_recorded: '18:00'},
    {data_ID: 12, sensor_ID: 3, channel_ID: 2, data_value: 29, data_unit: 'atm', time_recorded: '19:00'},
    {data_ID: 13, sensor_ID: 3, channel_ID: 1, data_value: 31, data_unit: 'atm', time_recorded: '20:00'}
]