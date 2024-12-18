"use client";
import React, { useEffect, useState } from "react";
import axios from "axios";
import Slider from "react-slick";
import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";
import { useRouter } from "next/navigation";

const Featured = () => {
  const [latestMovies, setLatestMovies] = useState([]);
  const apiUrl = process.env.NEXT_PUBLIC_NEW_API_URL;
  const router = useRouter();

  useEffect(() => {
    const fetchLatest = async () => {
      try {
        const response = await axios.get(`${apiUrl}/v1/Movies/latest?count=5`);

        if (response.status === 200) {
          console.log(response.data);
          setLatestMovies(response.data);
        }
      } catch (error) {
        console.log(error);
      }
    };

    fetchLatest();
  }, []);

  var settings = {
    dots: false,
    infinite: true,
    slidesToShow: 1,
    slidesToScroll: 1,
    autoplay: true,
    autoplaySpeed: 3000,
    pauseOnHover: true,
  };

  return (
    <div className="slider-container max-w-7xl mx-auto mt-10">
      <Slider {...settings}>
        {latestMovies.map((movie) => (
          <div key={movie.id} className="relative h-[500px]">
            {/* Background Image */}
            <div
              className="w-full h-full bg-cover bg-center rounded-lg"
              style={{
                backgroundImage: `url(${movie.picture})`, // Updated field for image
              }}
            ></div>

            {/* Overlay Content */}
            <div className="absolute bottom-6 left-6 bg-black bg-opacity-70 text-white p-6 rounded-lg max-w-lg">
              <h2 className="text-2xl font-bold">{movie.name}</h2>
              <p className="text-base text-gray-300 mt-2">
                {movie.dateCreated ? movie.dateCreated : "1970"}
              </p>
              <p className="text-sm mt-1 line-clamp-3 text-white">
                {movie.description
                  ? movie.description
                  : "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. "}
              </p>
              <div className="mt-4 flex justify-between items-center">
                <span className="bg-gray-700 text-sm font-medium px-4 py-2 rounded-lg">
                  {movie.genres.name}
                </span>
                <span>
                  <button
                    className="bg-white text-black text-sm font-medium px-4 py-2 rounded-lg"
                    onClick={() => router.push(`/movie/${movie.id}`)}
                  >
                    View Details
                  </button>
                </span>
              </div>
            </div>
          </div>
        ))}
      </Slider>
    </div>
  );
};

export default Featured;
