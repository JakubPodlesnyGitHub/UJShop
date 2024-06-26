import React from "react";
import ReactDOM from "react-dom";
import { Provider } from "react-redux";
import "bootstrap/dist/css/bootstrap.min.css";
import "./index.css";

import { PersistGate } from "redux-persist/integration/react";
import { store, persistor } from "./state/store";
import { AuthProvider } from "./context/AuthContext";
import App from "./App";

ReactDOM.render(
    <AuthProvider>
        <Provider store={store}>
            <PersistGate loading={null} persistor={persistor}>
                <App />
            </PersistGate>
        </Provider>
    </AuthProvider>,
    document.getElementById("root"),
);
