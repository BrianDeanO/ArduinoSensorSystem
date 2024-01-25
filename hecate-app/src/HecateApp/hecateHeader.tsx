import React, { useState } from "react";
//@ts-ignore
import HecateLogo  from "../images/Hecate_Logo.png";
// import HecateLogo from "../images/"
import { version } from "../Variables";

const HecateHeader = () => {
    const [loggedIn, setLoggedIn] = useState<boolean>(false);
    return (
        <div className="MainHecateHeader">
            
            <div className="ImageBox">
                <div className="versionBox">
                    Version {version}
                </div>
                <img src={HecateLogo} className="HecateLogo" alt="Hecate Software Logo" />
            </div>
            <div className="MainHeaderLogin">
                <button 
                    className="mainLoginButton"
                    onClick={() => {
                        setLoggedIn(!loggedIn);
                        loggedIn ? 
                            alert("You have been logged out.") : 
                            alert("You have been logged in.")
                    }}>
                        {loggedIn ? 'Log Out': 'Log In'}
                </button>
            </div>
        </div>
    )
}

export default HecateHeader;