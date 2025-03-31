window.onload = function() {
  window.ui = SwaggerUIBundle({
      url: "swagger.json", // Ensure this file is in the dist folder
      dom_id: "#swagger-ui",
      deepLinking: true,
      presets: [
          SwaggerUIBundle.presets.apis,
          SwaggerUIStandalonePreset
      ],
      layout: "StandaloneLayout"
  });
};
