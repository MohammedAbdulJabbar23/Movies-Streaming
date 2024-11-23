import React from 'react';

const Loader = () => {
  return (
    <div className="flex items-center justify-center w-20 h-20">
      <style>
        {`
          :root {
            --uib-size: 25px;
            --uib-speed: 1.5s;
          }
          @keyframes rotate936 {
            0% {
              transform: rotate(0deg);
            }
            100% {
              transform: rotate(360deg);
            }
          }
          @keyframes orbit {
            0% {
              transform: translate(calc(var(--uib-size) * 0.5)) scale(0.73684);
              opacity: 0.65;
            }
            25% {
              transform: translate(0%) scale(0.47368);
              opacity: 0.3;
            }
            50% {
              transform: translate(calc(var(--uib-size) * -0.5)) scale(0.73684);
              opacity: 0.65;
            }
            75% {
              transform: translate(0%) scale(1);
              opacity: 1;
            }
            100% {
              transform: translate(calc(var(--uib-size) * 0.5)) scale(0.73684);
              opacity: 0.65;
            }
          }
        `}
      </style>
      <div
        className="relative flex items-center justify-center w-[var(--uib-size)] h-[var(--uib-size)] animate-[rotate936_calc(var(--uib-speed)_*_1.667)_infinite_linear]"
      >
        <div className="absolute w-[60%] h-[60%] bg-white rounded-full animate-[orbit_var(--uib-speed)_linear_infinite]" />
        <div className="absolute w-[60%] h-[60%] bg-white rounded-full animate-[orbit_var(--uib-speed)_linear_calc(var(--uib-speed)_/_-2)_infinite]" />
      </div>
    </div>
  );
};

export default Loader;
