import { useState } from "react";
import Header from "./Header.tsx";
import Body from "./Body.tsx";
import { ADMIN, localStorageTitles } from "../variables.js";

const App = () => {
    const userJSON = localStorage.getItem(localStorageTitles.currentUser);
    const loggedInUser = (userJSON !== null) ? JSON.parse(userJSON) : {};
    
    const [loggedInUserID, setLoggedInUserID] = useState((Object.keys(loggedInUser).length > 1) ? loggedInUser.currentUserID : 0);
    const [isAdmin, setIsAdmin] = useState((Object.keys(loggedInUser).length > 1) ? loggedInUser.isAdmin : false);
    const [isManagingUsers, setIsManagingUsers] = useState(false);
    const [isLoggingOut, setIsLoggingOut] = useState(false);

    const logIn = (user) => {
        if((Object.keys(user).length === 0)) {
            localStorage.setItem(localStorageTitles.currentUser, JSON.stringify({}));
            setIsLoggingOut(true);
            setIsManagingUsers(false);
        } else {
            localStorage.setItem(localStorageTitles.currentUser, JSON.stringify({
                currentUserID: user.userID,
                currentFirstName: user.userFirstName,
                currentLastName: user.userLastName,
                isAdmin: (user.userType ? (user.userType.toUpperCase() === ADMIN) : false)
            }));
            setIsLoggingOut(false);
        }
        setLoggedInUserID(user.userID);
        setIsAdmin((user.userType ? (user.userType.toUpperCase() === ADMIN) : false));
    }

    const manageUsers = (manageUsers) => {
        setIsManagingUsers(manageUsers);
    }

    return (
        <div className="MainPage">            
            <Header 
                logIn={logIn} 
                loggedInUser={loggedInUser} 
                isAdmin={isAdmin} 
                manageUsers={manageUsers}
            />
            <Body 
                loggedInUserID={loggedInUserID} 
                isAdmin={isAdmin} 
                manageUsers={manageUsers} 
                isManagingUsers={isManagingUsers}
                isLoggingOut={isLoggingOut}
            />
        </div>
    )
}

export default App;