"use client";
import React from "react";
import { useState, useEffect } from "react";
import axios from "axios";
import Slider from "./Slider";

const Movie = () => {
  const apiUrl = process.env.NEXT_PUBLIC_NEW_API_URL;
  const [movies, setMovies] = useState([]);

  const fetchMovies = async () => {
    const response = await axios.get(`${apiUrl}/v1/Movies`);
    // console.log(response.data);
    setMovies(response.data);
  };

  useEffect(() => {
    fetchMovies();
  }, []);

  return <div>{movies && <Slider items={movies} />}</div>;
};

export default Movie;
