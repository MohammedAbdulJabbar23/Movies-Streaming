import React from "react";
import Link from "next/link";

const UploadMovieButton = () => {
  return (
    <Link href="/upload-movie">
      <button className="fixed top-6 right-6 z-50 bg-gray-800 text-white text-sm p-2 py-3 rounded-full shadow-lg hover:bg-gray-600 transition-transform transform  flex items-center justify-center">
        UploadMovieButton
      </button>
    </Link>
  );
};

export default UploadMovieButton;
