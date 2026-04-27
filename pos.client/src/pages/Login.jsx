import { useState } from "react";
import axios from "axios";

export default function Login() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

    const handleLogin = async (e) => {
        e.preventDefault();

        try {
            const response = await axios.post(
                "http://localhost:5028/api/auth/login",
                {
                    email,
                    password,
                }
            );

            const token = response.data.token;

            //Store token here
            localStorage.setItem("token", token);

            alert("Login successful");

            // redirect (optional)
            window.location.href = "/dashboard";

        } catch (error) {
            console.error(error);
            alert("Invalid credentials");
        }
    };

    return (
        <form onSubmit={handleLogin}>
            <h2>Login</h2>

            <input
                type="email"
                placeholder="Email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
            />

            <input
                type="password"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
            />

            <button type="submit">Login</button>
        </form>
    );
}