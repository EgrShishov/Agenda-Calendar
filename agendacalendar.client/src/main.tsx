import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.tsx'
import './index.css'
import {GoogleOAuthProvider} from "@react-oauth/google";
import ContextWrapper from "./context/contextWrapper.tsx";

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
      <ContextWrapper>
          <GoogleOAuthProvider
              clientId="120211586213-asjq851j2kp1sortorifct6vv44eactp.apps.googleusercontent.com">
              <App />
          </GoogleOAuthProvider>
      </ContextWrapper>
  </React.StrictMode>
)
