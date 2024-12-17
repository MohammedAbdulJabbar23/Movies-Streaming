"use client";
import axios from "axios";
import React, { useState, useEffect } from "react";
import { useRouter } from "next/navigation";
import Cookies from "js-cookie";

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
  const [releaseDate, setReleaseDate] = useState("");
  const [file, setFile] = useState(null);
  const [isAdmin, setIsAdmin] = useState(false);
  const [loading, setLoading] = useState(true);
  const router = useRouter();

  useEffect(() => {
    const checkUserRole = async () => {
      try {
        const token = Cookies.get("token");
        if (!token) {
          router.push("/");
          return;
        }

        const response = await axios.get(`${apiUrl}/Users/UserRole`, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });

        if (response.data.role === "Admin") {
          setIsAdmin(true);
        } else {
          router.push("/unauthorized");
        }
      } catch (error) {
        console.error("Error fetching user role:", error);
        router.push("/login");
      } finally {
        setLoading(false);
      }
    };

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

    checkUserRole();
    getGenres();
    getSubGenres();
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (
      !name ||
      !description ||
      !picture ||
      !file ||
      !releaseDate ||
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
    formData.append("VideoFile", file);
    formData.append("GenreId", selectedGenre);
    formData.append("SubGenreId", selectedSubGenre);
    formData.append("DateCreated", releaseDate);

    try {
      const token = Cookies.get("token");
      const response = await axios.post(`${apiUrl}/Movies`, formData, {
        headers: {
          "Content-Type": "multipart/form-data",
          Authorization: `Bearer ${token}`,
        },
      });

      if (response.status === 201) {
        alert("Movie Added Successfully");
        router.push("./");
      }
    } catch (error) {
      console.log(error);
    }
  };

  if (loading) {
    return <div className="text-gray-100 text-center mt-20">Loading...</div>;
  }

  if (!isAdmin) {
    return null;
  }

  return (
    <div className="flex items-center justify-center min-h-screen text-gray-100 mt-24">
      <div className="w-[60vw] bg-gray-800 p-8 rounded-lg">
        <p className="text-center text-xl font-bold">Upload Movie</p>
        <form className="mt-6 flex flex-col gap-4" onSubmit={handleSubmit}>
          {/* Movie Name */}
          <div>
            <label className="block text-gray-400 mb-1 text-sm">
              Movie Name
            </label>
            <input
              type="text"
              value={name}
              onChange={(e) => setName(e.target.value)}
              className="w-full rounded-md border border-gray-700 bg-gray-800 p-2.5 text-gray-100 focus:border-purple-400 outline-none"
              placeholder="Enter movie name"
            />
          </div>

          {/* Movie Picture URL */}
          <div>
            <label className="block text-gray-400 mb-1 text-sm">
              Picture URL
            </label>
            <input
              type="text"
              value={picture}
              onChange={(e) => setPicture(e.target.value)}
              className="w-full rounded-md border border-gray-700 bg-gray-800 p-2.5 text-gray-100 focus:border-purple-400 outline-none"
              placeholder="Enter picture URL"
            />
          </div>

          {/* Movie Description */}
          <div>
            <label className="block text-gray-400 mb-1 text-sm">
              Description
            </label>
            <input
              type="text"
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              className="w-full rounded-md border border-gray-700 bg-gray-800 p-2.5 text-gray-100 focus:border-purple-400 outline-none"
              placeholder="Enter description"
            />
          </div>

          {/* Movie Release Data */}

          <div>
            <label className="block text-gray-400 mb-1 text-sm">
              Release Data
            </label>
            <input
              type="text"
              className="w-full rounded-md border border-gray-700 bg-gray-800 p-2.5 text-gray-100 focus:border-purple-400 outline-none"
              placeholder="Movie Release Date"
              onChange={(e) => setReleaseDate(e.target.value)}
            />
          </div>

          {/* Movie Rating */}
          <div>
            <label className="block text-gray-400 mb-1 text-sm">Rating</label>
            <input
              type="number"
              value={rating}
              onChange={(e) => setRating(e.target.value)}
              className="w-full rounded-md border border-gray-700 bg-gray-800 p-2.5 text-gray-100 focus:border-purple-400 outline-none"
              placeholder="Enter rating"
            />
          </div>

          {/* Movie File */}
          <div>
            <label className="block text-gray-400 mb-1 text-sm">
              Movie File
            </label>
            <input
              type="file"
              onChange={(e) => setFile(e.target.files[0])}
              className="w-full rounded-md border border-gray-700 bg-gray-800 p-2.5 text-gray-100 focus:border-purple-400 outline-none"
            />
          </div>

          {/* Genre */}
          <div>
            <label className="block text-gray-400 mb-1 text-sm">Genre</label>
            <select
              value={selectedGenre}
              onChange={(e) => setSelectedGenre(e.target.value)}
              className="w-full rounded-md border border-gray-700 bg-gray-800 p-2.5 text-gray-100 focus:border-purple-400 outline-none"
            >
              <option value="">-- Choose a Genre --</option>
              {genres.map((genre) => (
                <option key={genre.id} value={genre.id}>
                  {genre.name}
                </option>
              ))}
            </select>
          </div>

          {/* Sub-Genre */}
          <div>
            <label className="block text-gray-400 mb-1 text-sm">
              Sub-Genre
            </label>
            <select
              value={selectedSubGenre}
              onChange={(e) => setSelectedSubGenre(e.target.value)}
              className="w-full rounded-md border border-gray-700 bg-gray-800 p-2.5 text-gray-100 focus:border-purple-400 outline-none"
            >
              <option value="">-- Choose a Sub-Genre --</option>
              {subGenres.map((sub) => (
                <option key={sub.id} value={sub.id}>
                  {sub.name}
                </option>
              ))}
            </select>
          </div>

          {/* Submit Button */}
          <button
            type="submit"
            className="w-full bg-purple-500 text-gray-900 mt-4 p-2.5 rounded-md font-semibold"
          >
            Upload
          </button>
        </form>
      </div>
    </div>
  );
};

export default page;
