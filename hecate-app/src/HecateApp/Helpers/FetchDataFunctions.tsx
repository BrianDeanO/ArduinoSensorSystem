import React, { useState, useEffect } from "react";
import { apiInfo } from "../../Variables";

export function fetchUserList() {
    let userData = [];

    console.log('fetchING USER LIST')
    fetch(apiInfo.USERS, {
        method: 'GET'
    })
    .then(response => {
        return response.json()
    })
    .then(responseData => {
        userData = responseData;
    })
    .catch(error => console.log("Authorization Failed: " + error.message));

    return userData;
}

export async function fetchDeviceList() {
    let deviceData = [];

    console.log('fetchING DEVICE LIST', apiInfo.DEVICES)
    const dData = await fetch(apiInfo.DEVICES)
        .then((response) => {return response.json().then((responseData) => {
            console.log('responseData', responseData)
            deviceData = responseData
            console.log('DEVICE DATA', deviceData);
            return responseData;
                }).catch((error) => {
                    console.log("Authorization Failed: " + error.message)
            })
        });

    console.log('dData', dData);
    return dData;
}

export function fetchUserDevicesList() {
    let userDevicesData = [];

    console.log('fetchING USER DEVICES LIST')
    fetch(apiInfo.USER_DEVICES, {
        method: 'GET'
    })
    .then(response => {
        return response.json()
    })
    .then(responseData => {
        userDevicesData = responseData;
    })
    .catch(error => console.log("Authorization Failed: " + error.message));

    return userDevicesData;
}

export function fetchUserDevice(userID: number, deviceID: number ) {
    let userDevice = {};

    console.log('fetchING USER DEVICE')
    fetch(`${apiInfo.USER_DEVICES}/${userID}:${deviceID}`, {
        method: 'GET'
    })
    .then(response => {
        return response.json()
    })
    .then(responseData => {
        userDevice = responseData;
    })
    .catch(error => console.log("Authorization Failed: " + error.message));

    return userDevice;
}