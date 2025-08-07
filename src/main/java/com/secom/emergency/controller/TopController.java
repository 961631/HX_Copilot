package com.secom.emergency.controller;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;

@Controller
public class TopController {
    
    /**
     * トップ画面を表示
     */
    @GetMapping("/")
    public String index() {
        return "top";
    }
    
    /**
     * トップ画面へのリダイレクト
     */
    @GetMapping("/top")
    public String top() {
        return "top";
    }
}