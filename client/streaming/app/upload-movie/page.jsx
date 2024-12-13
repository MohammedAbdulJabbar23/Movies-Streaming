"use client";
import axios from "axios";
import React, { useState, useEffect } from "react";
import { useRouter } from "next/navigation";

const page = () => {
  const apiUrl = process.env.NEXT_PUBLIC_NEW_API_URL;
  const [genres, setGenres] = useState([]);
  const [selectedGenre, setSelectedGenre] = useState("");
  const [subGenres, setSubGenres] = useState([]);
  const [selectedSubGenre, setSelectedSubGenre] = useState("");
  const [name, setName] = useState("");
  const [picture, setPicture] = useState("");
  const [description, setDescription] = useState("");
  const [rating, setRating] = useState(0);
  const [file, setFile] = useState(null);
  const router = useRouter();

  useEffect(() => {
    const getGenres = async () => {
      try {
        const response = await axios.get(`${apiUrl}/Genres`);
        if (response.status === 200) {
          setGenres(response.data);
        }
      } catch (error) {
        console.log(error);
      }
    };

    const getSubGenres = async () => {
      try {
        const response = await axios.get(`${apiUrl}/SubGenres`);
        if (response.status === 200) {
          setSubGenres(response.data);
        }
      } catch (error) {
        console.log(error);
      }
    };

    getGenres();
    getSubGenres();
  }, []);

  const handleGenreChange = (e) => {
    setSelectedGenre(e.target.value);
  };

  const handleSubGenreChange = (e) => {
    setSelectedSubGenre(e.target.value);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (
      !name ||
      !description ||
      !picture ||
      !file ||
      !selectedGenre ||
      !selectedSubGenre ||
      !rating
    ) {
      alert("Please fill in all the required fields.");
      return;
    }

    const currentDate = new Date();
    const isoString = currentDate.toISOString();

    const formData = new FormData();
    formData.append("Name", name);
    formData.append("Picture", picture);
    formData.append("Description", description);
    formData.append("Rating", rating);
    formData.append("DateCreated", isoString);
    formData.append("VideoFile", file);
    formData.append("GenreId", selectedGenre);
    formData.append("SubGenreId", selectedSubGenre);

    try {
      const response = await axios.post(`${apiUrl}/Movies`, formData, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      });

      console.log(response.data);

      if (response.status === 201) {
        alert("Movie Added Successfully");
        router.push("./");
      }
    } catch (error) {
      console.log(error);
    }
  };

  return (
    <div className="mt-24 px-8 min-h-screen">
      <h1 className="text-3xl font-bold text-center text-white mb-8">
        Upload Movies
      </h1>
      <div className="max-w-lg mx-auto bg-gray-800 p-6 rounded-lg shadow-lg">
        <form className="flex flex-col gap-6" onSubmit={handleSubmit}>
          {/* Movie Name */}
          <div>
            <label className="block text-gray-200 font-semibold mb-2">
              Movie Name
            </label>
            <input
              type="text"
              placeholder="Enter movie name"
              onChange={(e) => setName(e.target.value)}
              className="w-full p-3 bg-gray-700 text-white border border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-400"
            />
          </div>

          {/* Movie Picture */}
          <div>
            <label className="block text-gray-200 font-semibold mb-2">
              Movie Picture URL
            </label>
            <input
              type="text"
              placeholder="Enter movie picture URL"
              onChange={(e) => setPicture(e.target.value)}
              className="w-full p-3 bg-gray-700 text-white border border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-400"
            />
          </div>

          {/* Movie Description */}
          <div>
            <label className="block text-gray-200 font-semibold mb-2">
              Movie Description
            </label>
            <input
              type="text"
              placeholder="Enter movie description"
              onChange={(e) => setDescription(e.target.value)}
              className="w-full p-3 bg-gray-700 text-white border border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-400"
            />
          </div>

          {/* Movie Rating */}
          <div>
            <label className="block text-gray-200 font-semibold mb-2">
              Movie Rating
            </label>
            <input
              type="number"
              placeholder="Enter movie rating"
              onChange={(e) => setRating(e.target.value)}
              className="w-full p-3 bg-gray-700 text-white border border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-400"
            />
          </div>

          {/* Movie File */}
          <div>
            <label className="block text-gray-200 font-semibold mb-2">
              Movie File
            </label>
            <input
              type="file"
              onChange={(e) => setFile(e.target.files[0])}
              className="w-full p-3 bg-gray-700 text-white border border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-400"
            />
          </div>

          {/* Genre Selection */}
          <div>
            <label className="block text-gray-200 font-semibold mb-2">
              Select Genre
            </label>
            <select
              name="genre"
              value={selectedGenre}
              onChange={handleGenreChange}
              className="w-full p-3 bg-gray-700 text-white border border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-400"
            >
              <option value="" disabled>
                -- Choose a Genre --
              </option>
              {genres.map((genre) => {
                return (
                  <option
                    key={genre.id}
                    value={genre.id}
                    className="bg-gray-700 text-white"
                  >
                    {genre.name}
                  </option>
                );
              })}
            </select>
          </div>

          {/* Sub-Genre Selection */}
          <div>
            <label className="block text-gray-200 font-semibold mb-2">
              Select Sub-Genre
            </label>
            <select
              name="sub-genre"
              value={selectedSubGenre}
              onChange={handleSubGenreChange}
              className="w-full p-3 bg-gray-700 text-white border border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-400"
            >
              <option value="" disabled>
                -- Choose a Sub-Genre --
              </option>
              {subGenres.map((sub) => {
                return (
                  <option
                    key={sub.id}
                    value={sub.id}
                    className="bg-gray-700 text-white"
                  >
                    {sub.name}
                  </option>
                );
              })}
            </select>
          </div>

          {/* Submit Button */}
          <button
            type="submit"
            className="mt-6 py-3 bg-blue-600 text-white font-semibold rounded-lg hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500"
          >
            Submit
          </button>
        </form>
      </div>
    </div>
  );
};

export default page;
