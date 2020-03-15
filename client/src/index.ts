"use strict";
// @ts-ignore
import { Elm } from "./elm/Main";
import "bulma";
import "./assets/sass/styles.scss";

(function () {
  const node: HTMLElement | null = document.getElementById("elm");
  const app = Elm.Main.init({
    node: node,
    flags: null
  });
})();
