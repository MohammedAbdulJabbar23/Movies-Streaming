"use client";
import axios from "axios";
import Cookies from "js-cookie";
import React, { useState, useEffect } from "react";

const Page = ({ params }) => {
  const { id } = React.use(params); // Ensure `params` is being passed correctly
  const [userDetails, setUserDetails] = useState({});
  const apiUrl = process.env.NEXT_PUBLIC_NEW_API_URL;

  useEffect(() => {
    const fetchUserDetails = async () => {
      try {
        const response = await axios.get(`${apiUrl}/Users/${id}`);

        console.log("User details:", response.data);
        setUserDetails(response.data);
      } catch (error) {
        console.error("API Error:", error.response || error.message);
      }
    };

    if (id) {
      fetchUserDetails();
    }
  }, [id]);

  return (
    <div>
      <h1>Profile Page</h1>
      <pre>{JSON.stringify(userDetails, null, 2)}</pre>
    </div>
  );
};

export default Page;
