import React from "react";

const Featured = () => {
  return (
    <div className="relative w-full h-[90vh]">
      <img
        src="https://th.bing.com/th/id/R.7cbd7d79ca76f28431c74f7b7d3a543d?rik=vkSKn4Y7GVJGdw&riu=http%3a%2f%2fwww.baltana.com%2ffiles%2fwallpapers-20%2fMovie-Character-Joker-Wallpaper-7516.jpg&ehk=X9pcRuHi5EwB71TYOwL%2fOLF%2f94g%2brA3vHzyCIMlWScs%3d&risl=&pid=ImgRaw&r=0"
        alt="Background"
        className="w-full h-full object-cover opacity-95"
      />
      <div className="absolute bottom-10 left-10 bg-black bg-opacity-25 rounded-md">
        <div className="w-[55vw] flex h-[45vh] p-3">
          <div className="w-50 mr-6">
            <img
              src="https://th.bing.com/th/id/OIP.-ctNmAd2plA0zNUqeHL0DgHaKf?rs=1&pid=ImgDetMain"
              alt="movie pic"
              className="h-full w-full object-cover rounded-lg"
            />
          </div>
          <div>
            <h1 className="text-white text-3xl font-bold mb-2">Joker</h1>
            <span className="flex gap-2">
              <h6>⭐ 6.5</h6>
              <h6>📅 2024-6-12</h6>
              <h6>📽️ Movie</h6>
            </span>
            <span className="flex gap-2 mt-1 mb-5">
              <h4 className="bg-gray-800 rounded-md p-1 text-xs grid items-center">
                Comedy
              </h4>
              <h4 className="bg-gray-800 rounded-md p-1 text-xs grid items-center">
                Horror
              </h4>
              <h4 className="bg-gray-800 rounded-md p-1 text-xs grid items-center">
                Drama
              </h4>
              <h4 className="bg-gray-800 rounded-md p-1 text-xs grid items-center">
                Adventure
              </h4>
              <h4 className="bg-gray-800 rounded-md p-1 text-xs grid items-center">
                Crime
              </h4>
            </span>
            <span>
              <p className="text-sm w-96">
                Lorem ipsum dolor sit amet consectetur adipisicing elit.
                Voluptates, illo cum? Dolore molestiae magni odio culpa, ducimus
                consectetur cumque nobis rem nihil.
              </p>
            </span>
            <span className="flex gap-3 mt-8">
              <button className="bg-white text-black rounded-md p-1 text-sm w-24 h-8">
                Play Trailer
              </button>
              <button className="bg-black text-white rounded-md p-1 border border-white text-sm w-24 h-8">
                More Info
              </button>
            </span>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Featured;
