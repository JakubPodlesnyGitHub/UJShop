import React, { useState, useContext } from "react";
import { Button, Form, Container, Alert } from "react-bootstrap";
import { AuthContext } from "../context/AuthContext";
import { useNavigate } from "react-router-dom";

export default function SignUp() {

  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [email, setEmail] = useState('');
  const [phoneNumber, setPhoneNumber] = useState('');
  const [password, setPassword] = useState('');
  const [isAdmin, setIsAdmin] = useState(false);
  //register fail/success message
  const [message, setMessage] = useState('');
  const [variant, setVariant] = useState('');
  const [showAlert, setShowAlert] = useState(false);

  const { login } = useContext(AuthContext);
  const navigate = useNavigate();

  const handleRegister = async (e) => {
    e.preventDefault();
    const response = await fetch("http://localhost:5164/api/Auth/register", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({
            firstName: firstName,
            lastName: lastName,
            birthDate: "2024-06-20T20:24:21.747Z", //to simplify use string here
            picture: "string", //to simplify use string here
            email: email,
            phoneNumber: phoneNumber,
            password: password,
            isAdmin: false,
        }),
    });
    if (response.ok) {
      const data = await response.json();
      if (data.isSucceeded === true) {
        setMessage('Registration successful!');
        setVariant('success');
        setShowAlert(true);

        login(data.token);
        navigate("/p5/");
      } else {
        setMessage(data.errorDetails);
        setVariant('danger');
        setShowAlert(true);
      }
  } else {
      setMessage('Registration failed');
      setVariant('danger');
      setShowAlert(true);
  }
};

  return (
    <Container>
      <div className="frame_spacing">
        {message && <Alert variant={variant} onClose={() => setShowAlert(false)} dismissible>{message}</Alert>}
        <h2 className="form_title">Sign up</h2>
        <Form className="between_fields_spacing" onSubmit={handleRegister}>
          <Form.Group className="between_fields_spacing" controlId="firstName">
            <Form.Label className="text-center">First Name</Form.Label>
            <Form.Control type="text" placeholder="Enter Your first name" value={firstName} onChange={(e) => setFirstName(e.target.value)} required/>
          </Form.Group>
          <Form.Group className="between_fields_spacing" controlId="lastName">
            <Form.Label className="text-center">Last Name</Form.Label>
            <Form.Control type="text" placeholder="Enter Your last name" value={lastName} onChange={(e) => setLastName(e.target.value)} required/>
          </Form.Group>
          <Form.Group className="between_fields_spacing" controlId="email">
            <Form.Label className="text-center">Email address</Form.Label>
            <Form.Control type="email" placeholder="Enter email" value={email} onChange={(e) => setEmail(e.target.value)} required/>
          </Form.Group>
          <Form.Group className="between_fields_spacing" controlId="phoneNumber">
            <Form.Label className="text-center">Phone number</Form.Label>
            <Form.Control type="text" placeholder="Enter phone number e.g. 777-777-777" value={phoneNumber} onChange={(e) => setPhoneNumber(e.target.value)} required/>
          </Form.Group>
          <Form.Group className="between_fields_spacing" controlId="password">
            <Form.Label>Password</Form.Label>
            <Form.Control type="password" placeholder="Password" value={password} onChange={(e) => setPassword(e.target.value)} required/>
          </Form.Group>
          <Form.Group controlId="formRole">
            <Form.Label>Role</Form.Label>
            <Form.Control
              as="select"
              value={isAdmin ? 'admin' : 'user'}
              onChange={(e) => setIsAdmin(e.target.value === 'admin')}
            >
              <option value="user">User</option>
              <option value="admin">Admin</option>
            </Form.Control>
          </Form.Group>
          <div className="centered">
            <Button variant="primary" size="lg" type="submit">
              SIGN UP
            </Button>
          </div>
        </Form>
      </div>
    </Container>
  );
}
